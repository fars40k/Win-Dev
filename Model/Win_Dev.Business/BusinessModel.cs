using System;
using System.Collections.Generic;
using Win_Dev.Data;
using System.Linq;

namespace Win_Dev.Business
{
    /// <summary>
    /// Contains all intermediate methods conecting UI and data access layers
    /// </summary>
    public partial class BusinessModel
    {
        public static DataAccessObject DataAccessObject { get; private set; } 

        public BusinessModel()
        {
           
        }

        public void BusinessModelInit(DatabaseWorker newDatabaseWorker)
        {

            if (DataAccessObject == null) DataAccessObject = newDatabaseWorker.DataAccessObject;

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
            List<BusinessProject> businessProjects = new List<BusinessProject>();

            Exception error = null;

            try
            {
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

        public void UpdateProject(BusinessProject projectFromUI,Action<Exception> callback)
        {
            Project found = DataAccessObject.Projects.FindByID(projectFromUI.ProjectID);

            Exception error = null;

            if (found != null)
            {

                found.Name = projectFromUI.Name;
                found.Description = projectFromUI.Description;
                found.CreationDate = projectFromUI.CreationDate;
                found.ExpireDate = projectFromUI.ExpireDate;
                found.Percentage = projectFromUI.Percentage;
                found.StatusKey = projectFromUI.StatusKey;

            }
            try
            {               
                DataAccessObject.Projects.SaveChanges();                
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
                IEnumerable<Goal> goalsForDelete = DataAccessObject.LinkedData.FindGoalsForProject(forDelete).ToList<Goal>();
                foreach(Goal item in goalsForDelete)
                {
                    DataAccessObject.Goals.Delete(item);
                }

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

            List<BusinessPerson> businessPersonel = new List<BusinessPerson>();

            try
            {

                List<Person> fromDataList = DataAccessObject.Personel.FindAll().ToList<Person>();       
        
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

        public void UpdatePersonel(IEnumerable<BusinessPerson> UIList, Action<Exception> callback)
        {
            Exception error = null;

            foreach (BusinessPerson item in UIList)
            {
                Person found = DataAccessObject.Personel.FindByID(item.PersonID);
                var s1 = DataAccessObject.LinkedData.CheckState(found);
                found.FirstName = item.FirstName;
                found.SurName = item.SurName;
                found.LastName = item.LastName;
                found.Division = item.Division;
                found.Occupation = item.Occupation;
                DataAccessObject.LinkedData.MakeModifiedStatus(found);

                try
                {
                    DataAccessObject.Personel.SaveChanges();
                }
                catch (Exception ex)
                {
                    error = ex;
                }
            }

            error = null;
            
           

            callback.Invoke(error);
        }

        public void DeletePerson(BusinessPerson forDelete,Action<Exception> callback)
        {
            
            Exception error = null;

            try
            {
                List<Person> fouundes = DataAccessObject.LinkedData.FindAllPersonelWithLinks().ToList();
                DataAccessObject.LinkedData.ClearLinksForPerson(forDelete.PersonID);
                DataAccessObject.LinkedData.SaveChanges();

                List<Person>  fouunde = DataAccessObject.LinkedData.FindAllPersonelWithLinks().ToList();
                Person found = DataAccessObject.Personel.FindByID(forDelete.PersonID);
                DataAccessObject.Personel.Delete(found);
            }
            catch (Exception ex)
            {
                error = ex;
            }
            
            callback.Invoke(error);
        }

        public void GetPersonelForProject(Guid projectGUID, Action<List<BusinessPerson>, Exception> callback)
        {
            List<BusinessPerson> businessPersonel = new List<BusinessPerson>();

            Exception error = null;

            try
            {

                IEnumerable<Person> fromDataList = DataAccessObject.LinkedData.FindPersonelForProject(projectGUID);

                if (fromDataList != null)
                {
                    foreach (Person item in fromDataList)
                    {
                        item.FirstName = item.FirstName.TrimEnd(' ');
                        item.SurName = item.SurName.TrimEnd(' ');
                        item.LastName = item.LastName.TrimEnd(' ');
                        item.Division = item.Division.TrimEnd(' ');
                        item.Occupation = item.Occupation.TrimEnd(' ');
                        businessPersonel.Add(new BusinessPerson(item));
                    }
                }

                error = null;
            }
            catch (Exception ex)
            {
                error = ex;
            }

            callback.Invoke(businessPersonel, error);

        }

        #endregion

        #region Goal_related

        public void CreateGoal(Guid forProject, Action<BusinessGoal, Exception> callback)
        {
            Exception error = null;

            BusinessGoal businessGoal = new BusinessGoal();

            try
            {
                businessGoal.Goal = new Goal();

                Random rnd = new Random();

                businessGoal.GoalID = Guid.NewGuid();

                businessGoal.Name = "Goal" + rnd.Next(0, 100).ToString();
                businessGoal.Description = "!";
                businessGoal.CreationDate = DateTime.Today;
                businessGoal.ExpireDate = businessGoal.CreationDate.AddDays(1);
                businessGoal.Percentage = 0;
                businessGoal.StatusKey = 0;
                businessGoal.Project.Add(DataAccessObject.Projects.FindByID(forProject)); 

                DataAccessObject.Goals.Insert(businessGoal.Goal);
                DataAccessObject.LinkedData.AddGoalToProject(businessGoal.GoalID, forProject);

                DataAccessObject.Goals.SaveChanges();

                error = null;
            }
            catch (Exception ex)
            {
                error = ex;
            }

            callback.Invoke(businessGoal, error);
        }

        public void GetPersonelForGoal(Guid goalGUID, Action<List<BusinessPerson>, Exception> callback)
        {
            List<BusinessPerson> businessPersonel = new List<BusinessPerson>();

            Exception error = null;

            try
            {

                IEnumerable<Person> fromDataList = DataAccessObject.LinkedData.FindPersonelForGoal(goalGUID);

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
        
        public void UpdateGoals(IEnumerable<BusinessGoal> UIList, Action<Exception> callback)
        {
        
                Exception error = null;
            try
            {
                foreach (BusinessGoal item in UIList)
                {
                    Goal found = DataAccessObject.Goals.FindByID(item.GoalID);

                    if (found != null)
                    {

                        found.Name = item.Name;
                        found.Description = item.Description;
                        found.CreationDate = item.CreationDate;
                        found.ExpireDate = item.ExpireDate;
                        found.Percentage = item.Percentage;
                        found.Priority = item.Priority;
                        found.StatusKey = item.StatusKey;

                    }
                }

                DataAccessObject.Goals.SaveChanges();
            }
            catch (Exception ex)
            {
                error = ex;
            }

            callback.Invoke(error);
        }

        public void DeleteGoal(BusinessGoal forDelete, Action<Exception> callback)
        {
            Exception error = null;

            try
            {
                Project goalInProject = forDelete.Project.FirstOrDefault<Project>();

                DataAccessObject.Goals.Delete(forDelete.Goal);
                DataAccessObject.LinkedData.RemoveGoalFromProject(forDelete.GoalID, goalInProject.ProjectID);
            }
            catch (Exception ex)
            {
                error = ex;
            }

            callback.Invoke(error);
        }

        public void GetGoalsListForProject(Guid ProjectGUID, Action<List<BusinessGoal>, Exception> callback)
        {

            List<BusinessGoal> businessGoals = new List<BusinessGoal>();

            Exception error = null;

            try
            {

                IEnumerable<Goal> fromDataList = DataAccessObject.LinkedData.FindGoalsForProject(ProjectGUID);

                if (fromDataList != null)
                {
                    foreach (Goal item in fromDataList)
                    {
                        item.Name = item.Name.TrimEnd(' ');
                        item.Description = item.Description.TrimEnd(' ');

                        businessGoals.Add(new BusinessGoal(item));
                    }
                }
                error = null;
            }
            catch (Exception ex)
            {
                error = ex;
            }

            callback.Invoke(businessGoals, error);
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

        /// <summary>
        /// Provide goals personel deletion when a person's project assignation removed
        /// </summary>
        public void UnassignPersonFromProject(Guid personGUID, Guid projectGUID, Action<Exception> callback)
        {
            Exception error = null;

            try
            {
                DataAccessObject.LinkedData.RemovePersonFromProject(personGUID, projectGUID);
                List<Goal> goals = DataAccessObject.Goals.FindAll().ToList();

                foreach (Goal item in goals)
                {
                    UnassignPersonFromGoal(personGUID, item.GoalID, (err) =>
                    {
                    });
                }

                DataAccessObject.LinkedData.SaveChanges();
                error = null;
            }
            catch (Exception ex)
            {
                error = ex;
            }

            callback.Invoke(error);
        }

        public void AssignPersonToGoal(Guid personGUID, Guid goalGUID, Action<Exception> callback)
        {
            Exception error = null;

            try
            {
                DataAccessObject.LinkedData.AddPersonToGoal(personGUID, goalGUID);
                DataAccessObject.LinkedData.SaveChanges();
                error = null;
            }
            catch (Exception ex)
            {
                error = ex;
            }

            callback.Invoke(error);
        }

        public void UnassignPersonFromGoal(Guid personGUID, Guid goalGUID, Action<Exception> callback)
        {
            Exception error = null;

            try
            {
                DataAccessObject.LinkedData.RemovePersonFromGoal(personGUID, goalGUID);
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

