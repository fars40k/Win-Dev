using System;
using System.Collections.Generic;
using Win_Dev;
using GalaSoft.MvvmLight;
using Win_Dev.Data;
using System.Linq;

namespace Win_Dev.Business
{
    /// <summary>
    /// Contains all intermediate methods conecting UI and data access layers
    /// </summary>
    public class BusinessModel
    {
        public static DataAccessObject DataAccessObject { get; private set; }

        private List<BusinessPerson> businessPersonel = new List<BusinessPerson>();
        private List<BusinessProject> businessProjects = new List<BusinessProject>();

        public BusinessModel()
        {
           
        }

        public void BusinessModelInit(DatabaseWorker newDatabaseWorker)
        {

            DataAccessObject = (DataAccessObject == null) ? newDatabaseWorker.DataAccessObject 
                                                            : DataAccessObject;
        }

        #region Project_related

        public void CreateProject(Action<BusinessProject,Exception> callback)
        {
            Exception error = null;

            BusinessProject project = new BusinessProject();

            try
            {
                Random rnd = new Random();
               
                project.Project = new Project();

                string projectName = "Project-" + rnd.Next(0, 1000).ToString();
                project.Name = projectName;
                project.CreationDate = DateTime.Today;
                project.ExpireDate = project.CreationDate.AddDays(1);

                project.ProjectID = Guid.NewGuid();
                project.Description = "!";
                project.Percentage = 0;
                project.StatusKey = 0;

                DataAccessObject.Projects.Insert(project.Project);

                DataAccessObject.Projects.SaveChanges();

                error = null;
            }
            catch (Exception ex)
            {
                error = ex;
            }

            callback.Invoke(project,error);
        }

        public void GetProjectsList(Action<List<BusinessProject>, Exception> callback)
        {
            Exception error = null;

            try
            {
                businessProjects.Clear();

                List<Project> fromDataList = DataAccessObject.Projects.FindAll().ToList<Project>();

                foreach (Project item in fromDataList)
                {
                    item.Name = item.Name.TrimEnd(' ');
                    item.Description = item.Description.TrimEnd(' ');

                    businessProjects.Add(new BusinessProject(item));
                }
                error = null;
            }
            catch (Exception ex)
            {
                error = ex;
            }

            callback.Invoke(businessProjects, error);
        }

        public void UpdateProjects(List<BusinessProject> projectsFromUI,Action<Exception> callback)
        {
            Exception error = null;

            try
            {
                foreach (BusinessProject item in projectsFromUI)
                {
                    Project fromDataProject = DataAccessObject.Projects.FindByID(item.ProjectID);

                    if (fromDataProject != null)
                    {
                        DataAccessObject.Projects.Update(item.Project);
                    }
                    else
                    {
                        DataAccessObject.Projects.Insert(item.Project);
                    }

                }

                          
            }
            catch (Exception ex)
            {
                error = ex;
            }

            callback.Invoke(error);
        }

        public void DeleteProject(Guid forDelete, Action<Exception> callback)
        {
            Exception error = null;

            try
            {
                DataAccessObject.Projects.Delete(forDelete);
                DataAccessObject.Projects.SaveChanges();
            }
            catch (Exception ex)
            {
                error = ex;
            }

            callback.Invoke(error);
        }

        public void GetProjectByID(Guid findID, Action<BusinessProject,Exception> callback)
        {
            Exception error = null;

            BusinessProject foundProject = null;

            try
            {           
                foundProject = new BusinessProject(DataAccessObject.Projects.FindByID(findID));
            }
            catch (Exception ex)
            {
                error = ex;
            }

            callback.Invoke(foundProject,error);
        }
    

        #endregion

        #region Personel_related

        public void CreatePerson(Action<BusinessPerson, Exception> callback)
        {
            Exception error = null;

            BusinessPerson businessPerson = new BusinessPerson();

            try
            {
                businessPerson.Person = new Person();

                Random rnd = new Random();
                businessPerson.PersonID = Guid.NewGuid();
                businessPerson.FirstName = "First" + rnd.Next(0, 100).ToString();
                businessPerson.SurName = "Sur" + rnd.Next(0, 100).ToString();
                businessPerson.LastName = "Last" + rnd.Next(0, 100).ToString();
                businessPerson.Division = "div" + rnd.Next(0, 100).ToString();
                businessPerson.Occupation = "occ" + rnd.Next(0, 100).ToString();
                businessPerson.Projects = new List<Project>();
                businessPerson.Goals = new List<Goal>();

                DataAccessObject.Personel.Insert(businessPerson.Person);

                DataAccessObject.Personel.SaveChanges();

                error = null;
            }
            catch (Exception ex)
            {
                error = ex;
            }

            callback.Invoke(businessPerson, error);
        }

        public void GetPersonelList(Action<List<BusinessPerson>, Exception> callback)
        {
            Exception error = null;

            try
            {
                
                List<Person> fromDataList = DataAccessObject.Personel.FindAll().ToList<Person>();

                businessPersonel.Clear();

                foreach (Person item in fromDataList)
                {
                    item.FirstName = item.FirstName.TrimEnd(' ');
                    item.SurName = item.SurName.TrimEnd(' ');
                    item.LastName = item.LastName.TrimEnd(' ');
                    item.Division = item.Division.TrimEnd(' ');
                    item.Occupation = item.Occupation.TrimEnd(' ');

                    businessPersonel.Add(new BusinessPerson(item));
                }
                error = null;
            }
            catch (Exception ex)
            {
                error = ex;
            }

            callback.Invoke(businessPersonel, error);
        }

        /// <summary>
        /// Compares the received collection with the dataccess entity model and makes the necessary updates
        /// </summary>
        public void UpdatePersonelList(IEnumerable<BusinessPerson> UIList, Action<Exception> callback)

        {
            Exception error = null;

            List<Person> uiListInput = new List<Person>();

            foreach (BusinessPerson item in UIList)
            {
                uiListInput.Add(item.Person);
            }

            try
            {
                List<Person> personelDataAccessList = new List<Person>(DataAccessObject.Personel.FindAll());

                List<Person> odds = uiListInput.Except(personelDataAccessList).ToList();

                foreach (Person item in odds)
                {
                    // Adding or updating entries
                    
                    if (DataAccessObject.Personel.FindByID(item.PersonID) != null)
                    {
                        if (uiListInput.Contains(item))
                        {

                            DataAccessObject.Personel.Update(item);

                        }
                    }
                    else
                    {

                        DataAccessObject.Personel.Insert(item);

                    }

                }            

                DataAccessObject.Personel.SaveChanges();

                GetPersonelList((list, errors) => 
                {


                });

                error = null;
            }
            catch (Exception ex)
            {
                error = ex;
            }

            callback.Invoke(error);
        }

        public void DeletePerson(BusinessPerson forDelete,Action<Exception> callback)
        {
            Exception error = null;

            try
            {
                DataAccessObject.Personel.Delete(forDelete.PersonID);              
            }
            catch (Exception ex)
            {
                error = ex;
            }
            
            callback.Invoke(error);
        }

        public void GetPersonelForProject(Guid projectGUID, Action<List<BusinessPerson>, Exception> callback)
        {
            Exception error = null;

            Project project = DataAccessObject.Projects.FindByID(projectGUID);

            List<BusinessPerson> businessPersonelForProject = new List<BusinessPerson>();

            if (project != null)
            {
                try
                {
                    List<Person> dataPersonelList = (List<Person>)DataAccessObject.Personel.FindAll();         

                    foreach (Person item in dataPersonelList)
                    {
                        if (item.Projects.Contains(project))
                            businessPersonelForProject.Add(new BusinessPerson(item));
                    }

                    error = null;
                }
                catch (Exception ex)
                {
                    error = ex;
                }
            }            

            callback.Invoke(businessPersonelForProject, error);
        }

        #endregion

        public void AssignPersonToProject(Guid personGUID, Guid projectGUID, Action<Exception> callback)
        {
            Exception error = null;

            try
            {
                DataAccessObject.LinkedData.AddPersonToProject(personGUID, projectGUID);
                DataAccessObject.LinkedData.SaveChanges();
                error = null;
            }
            catch (Exception ex)
            {
                error = ex;
            }

            callback.Invoke(error);
        }

        public void UnassignPersonToProject(Guid personGUID, Guid projectGUID, Action<Exception> callback)
        {
            Exception error = null;

            try
            {
                DataAccessObject.LinkedData.RemovePersonFromProject(personGUID, projectGUID);
                DataAccessObject.LinkedData.SaveChanges();
                error = null;
            }
            catch (Exception ex)
            {
                error = ex;
            }

            callback.Invoke(error);
        }
    }

}

