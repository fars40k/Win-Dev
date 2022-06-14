using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win_Dev.Data.Interfaces;

namespace Win_Dev.Data
{
    public class DataAccessObject : IDataAccessService
    {
        public IRepository<Person> Personel;
        public IRepository<Project> Projects;
        public IRepository<Goal> Goals;
        public LinkedDataRepository LinkedData;

        public DataAccessObject()
        {
            
        }

        public void UpdateContextInRepositories()
        {
            WinTaskContext wtContext = new WinTaskContext();

            Personel = new BaseRepository<Person>(wtContext);
            Projects = new BaseRepository<Project>(wtContext);
            Goals = new BaseRepository<Goal>(wtContext);
            LinkedData = new LinkedDataRepository(wtContext);

        }

        public void UpdateEntityModel()
        {
            using (WinTaskContext wtContext = new WinTaskContext())
            {
                Goals.FindAll();
                Personel.FindAll();
                Projects.FindAll();
            }
        }

        public void SaveChanges()
        {
            Personel.SaveChanges();

            UpdateContextInRepositories();
        }

    }
}
