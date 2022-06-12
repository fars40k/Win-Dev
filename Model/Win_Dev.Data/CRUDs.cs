using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;

namespace Win_Dev.Data
{
    /// <summary>
    /// Contains definitions of all operations with database
    /// </summary>
    class CRUDs
    {
        /// <summary>
        /// Searches the database for an entry specified by ID and Dto class, or returns null
        /// </summary>
        public object ReadEntryByID<T>(Guid ID) where T : class
        {
            T obj = default(T);

            using (WinTaskContext wC = new WinTaskContext())
            {
                if (obj is Goal)
                {
                    Goal res = wC.Goals.FirstOrDefault(i => i.GoalID == ID);
                    return res;
                }
                else

                if (obj is Personel)
                {
                    Personel res = wC.Personel.FirstOrDefault(i => i.PersonID == ID);
                    return res;
                }
                else

                if (obj is Project)
                {
                    Project res = wC.Projects.FirstOrDefault(i => i.ProjectID == ID);
                    return res;
                }
                else

                {
                    return null;
                }
            }
        }

        public object ReadEntriesList<T>() where T : class
        {
            T obj = default(T);

            using (WinTaskContext wC = new WinTaskContext())
            {
                if (obj is Goal)
                {
                    List<Goal> list = wC.Goals.ToList(); 
                    return list;
                }
                else

                if (obj is Personel)
                {
                    List<Personel> list = wC.Personel.ToList();
                    return list;
                }
                else

                if (obj is Project)
                {
                    List<Project> list = wC.Projects.ToList();
                    return list;
                }
                else

                {
                    return null;
                }
            }
        }

        public bool AddEntry<TEntity>(TEntity item)
        {
            return false;
        }
    }
}
