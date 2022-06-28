using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win_Dev.Data.Interfaces;

namespace Win_Dev.Data
{
    /// <summary>
    /// Contains Repositories with basic database operations
    /// </summary>
    public class DataAccessObject : IDataAccessService
    {
        public IRepository<Person> Personel;
        public IRepository<Project> Projects;
        public IRepository<Goal> Goals;
        public LinkedDataWorker LinkedData;

        public DataAccessObject()
        {
            
        }

        public void UpdateContextInRepositories()
        {
            WinTaskContext wtContext = new WinTaskContext();

            Personel = new BaseRepository<Person>(wtContext);
            Projects = new BaseRepository<Project>(wtContext);
            Goals = new BaseRepository<Goal>(wtContext);
            LinkedData = new LinkedDataWorker(wtContext);

        }

        public void UpdateEntityModel()
        {
            using (WinTaskContext wtContext = new WinTaskContext())
            {
                Personel.FindAll();
                var projects = wtContext.Personel.Include("ProjectsWith");
                var goals = wtContext.Personel.Include("GoalsWith");
                var goalsToProjects = wtContext.Goals.Include("ProjectsWith");
            }
        }

        public void SaveChanges()
        {
            Personel.SaveChanges();

            UpdateContextInRepositories();
        }

    }
}
