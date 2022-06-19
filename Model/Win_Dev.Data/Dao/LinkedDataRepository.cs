using System;
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

