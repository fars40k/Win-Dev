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
            var project = _context.Projects.Where(p => p.ProjectID.Equals(ProjectGUID));
            var person = _context.Personel.Where(r => r.PersonID.Equals(PersonGUID));

            Project projectDao = project as Project;
            Person personDao = person as Person;

            if ((projectDao != null) && (personDao != null) && (!projectDao.Personel.Contains<Person>(personDao)))
            {
                projectDao.Personel.Add(personDao);
            }
        }

        public void RemovePersonelFromProject(Guid PersonGUID, Guid ProjectGUID)
        {
            var project = _context.Projects.Where(p => p.ProjectID.Equals(ProjectGUID));
            var person = _context.Personel.Where(r => r.PersonID.Equals(PersonGUID));

            Project projectDao = project as Project;
            Person personDao = person as Person;

            if ((projectDao != null) && (personDao != null) && (projectDao.Personel.Contains<Person>(personDao)))
            {
                projectDao.Personel.Remove(personDao);
            }
        }

        public void SaveChanges()
        {
            _context.Configuration.AutoDetectChangesEnabled = true;
            _context.SaveChanges();
            _context.Configuration.AutoDetectChangesEnabled = false;
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

