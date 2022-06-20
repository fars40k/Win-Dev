﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win_Dev.Data.Interfaces;

namespace Win_Dev.Data
{

    public class LinkedDataRepository
    {
        private WinTaskContext _context;

        private bool disposed = false;

        public LinkedDataRepository(WinTaskContext context)
        {
            this._context = context;
        }

        public void AddPersonToProject(Guid PersonGUID, Guid ProjectGUID)
        {
            var project = _context.Projects.Where(p => p.ProjectID.Equals(ProjectGUID)).FirstOrDefault<Project>();
            var person = _context.Personel.Where(r => r.PersonID.Equals(PersonGUID)).FirstOrDefault<Person>();

            Project projectDao = project;
            Person personDao = person;

            if ((projectDao != null) && (personDao != null) && (!projectDao.PersonelWith.Contains<Person>(personDao)))
            {
                projectDao.PersonelWith.Add(personDao);
            }
        }

        public void RemovePersonFromProject(Guid PersonGUID, Guid ProjectGUID)
        {
            var project = _context.Projects.Where(p => p.ProjectID.Equals(ProjectGUID));
            var person = _context.Personel.Where(r => r.PersonID.Equals(PersonGUID));

            Project projectDao = project as Project;
            Person personDao = person as Person;

            if ((projectDao != null) && (personDao != null) && (projectDao.PersonelWith.Contains<Person>(personDao)))
            {
                projectDao.PersonelWith.Remove(personDao);
            }
        }

        public void AddGoalToProject(Guid GoalGUID, Guid ProjectGUID)
        {
            var project = _context.Projects.Where(p => p.ProjectID.Equals(ProjectGUID)).FirstOrDefault<Project>();
            var goal = _context.Goals.Where(r => r.GoalID.Equals(GoalGUID)).FirstOrDefault<Goal>();

            Project projectDao = project;
            Goal goalDao = goal;

            if ((projectDao != null) && (goalDao != null) && (!projectDao.GoalsIn.Contains<Goal>(goalDao)))
            {
                projectDao.GoalsIn.Add(goalDao);
            }
        }

        public void RemoveGoalFromProject(Guid GoalGUID, Guid ProjectGUID)
        {
            var project = _context.Projects.Where(p => p.ProjectID.Equals(ProjectGUID)).FirstOrDefault<Project>();
            var goal = _context.Goals.Where(r => r.GoalID.Equals(GoalGUID)).FirstOrDefault<Goal>();

            Project projectDao = project;
            Goal goalDao = goal;

            if ((projectDao != null) && (goalDao != null) && (projectDao.GoalsIn.Contains<Goal>(goalDao)))
            {
                projectDao.GoalsIn.Remove(goalDao);
            }
        }

        public void AddPersonToGoal(Guid PersonGUID, Guid GoalGUID)
        {
            var goal = _context.Goals.Where(p => p.GoalID.Equals(GoalGUID)).FirstOrDefault<Goal>();
            var person = _context.Personel.Where(r => r.PersonID.Equals(PersonGUID)).FirstOrDefault<Person>();

            Goal goalDao = goal;
            Person personDao = person;

            if ((goalDao != null) && (personDao != null) && (!goalDao.PersonelWith.Contains<Person>(personDao)))
            {
                goalDao.PersonelWith.Add(personDao);
            }
        }

        public void RemovePersonFromGoal(Guid PersonGUID, Guid GoalGUID)
        {
            var goal = _context.Goals.Where(p => p.GoalID.Equals(GoalGUID)).FirstOrDefault<Goal>();
            var person = _context.Personel.Where(r => r.PersonID.Equals(PersonGUID)).FirstOrDefault<Person>();

            Goal goalDao = goal;
            Person personDao = person;

            if ((goalDao != null) && (personDao != null) && (goalDao.PersonelWith.Contains<Person>(personDao)))
            {
                goalDao.PersonelWith.Remove(personDao);
            }
        }

        public IEnumerable<Goal> FindGoalsForProject(Guid ProjectID)
        {
            var project = _context.Projects.Where(p => p.ProjectID.Equals(ProjectID)).FirstOrDefault<Project>();

            Project projectDao = project;

            List<Goal> goals = new List<Goal>();

            var goalsDao = _context.Goals.Include(g => g.ProjectsWith).ToList();

            foreach (Goal item in goalsDao)
            {
                if (item.ProjectsWith.Contains(projectDao))
                    goals.Add(item);
            }

            return goals;
        }

        public IEnumerable<Person> FindPersonelForGoal(Guid GoalID)
        {
            var goal = _context.Goals.Where(g => g.GoalID.Equals(GoalID));
            IEnumerable<Person> personel = _context.Personel.Where(g => g.GoalsWith == goal);

            return personel;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;
            if (disposing)
            {

            }
            disposed = true;
        }

        ~LinkedDataRepository()
        {
            Dispose(false);
        }

    }

}

