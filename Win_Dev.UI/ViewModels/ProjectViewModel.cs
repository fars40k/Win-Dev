using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Win_Dev.Business;
using Win_Dev.Data;
using Win_Dev.UI.Views;

namespace Win_Dev.UI.ViewModels
{
    public class ProjectViewModel : ViewModelBase
    {
        public BusinessModel Model = SimpleIoc.Default.GetInstance<BusinessModel>();

        public BusinessProject Project;

        #region Project_properties

        public string ProjectID
        {
            get => Project.ProjectID.ToString();
            set
            {

            }
        }

        public string ProjectName
        {
            get => Project.Name;
            set
            {
                Project.Name = value;
                RaisePropertyChanged("ProjectName");
            }
        }

        public string Description
        {
            get => Project.Description;
            set
            {
                Project.Description = value;
                RaisePropertyChanged("Description");
            }
        }

        #region Project_dates

        public DateTime CreationDate
        {
            get
            {
                return Project.CreationDate;
            }
            set
            {
                Project.CreationDate = value;
                RaisePropertyChanged("CreationDate");
                DateChangedCommand.Execute(this);
            }
        }

        public DateTime ExpireDate
        {
            get
            {
                return Project.ExpireDate;
            }
            set
            {
                Project.ExpireDate = value;
                RaisePropertyChanged("ExpireDate");
                DateChangedCommand.Execute(this);
            }
        }

        private string _constructedCommentary
        {
            get
            {
                string obj = "";
                string subtraction = Project.ExpireDate.Subtract(Project.CreationDate).TotalDays.ToString();

                // If Expire date past start date

                if (Int32.Parse(subtraction) < 0)
                {
                    obj += Application.Current.Resources["Wrong_data"];
                }
                else
                {                
                    obj += Application.Current.Resources["Planned"] + " " + subtraction;
                    obj += " " + Application.Current.Resources["Days"] + ". ";

                    // Comparison with the today date

                    if (Int32.Parse(subtraction) >= 0)
                    {

                        obj += Application.Current.Resources["To_completion"] + " " + Math.Ceiling(Project.ExpireDate.Subtract(DateTime.Now).TotalDays);

                    }
                    else
                    {

                        obj += Application.Current.Resources["Late_for"] + " " + Math.Ceiling(Project.ExpireDate.Subtract(DateTime.Now).TotalDays);
                        obj += " (" + DateTime.Now + ") " + Application.Current.Resources["Days"] + ". ";
                    }
                }
                return obj;
            }
        }
        public string ConstructedCommentary
        {
            get
            {
                return _constructedCommentary;
            }
            set
            {
                RaisePropertyChanged("ConstructedCommentary");
            }
        }
        #endregion

        public byte Percentage
        {
            get => Project.Percentage;
            set
            {
                Project.Percentage = value;
                RaisePropertyChanged("Persentage");
            }
        }

        public int SelectedCondition
        {
            get => Project.StatusKey; 
            set
            {
                Project.StatusKey = value;
                RaisePropertyChanged("SelectedCondition");
            }
        }

        private ObservableCollection<string> _conditions;
        public ObservableCollection<string> Conditions
        {
            get { return _conditions; }
            set
            {
                _conditions = value;
                RaisePropertyChanged("Conditions");
            }
        }

        public ObservableCollection<BusinessPerson> Employees { get; set; }
        public ObservableCollection<BusinessPerson> ProjectEmployees { get; set; }

        #endregion

        private UserControl _goalsView;
        public UserControl GoalsView
        {
            get { return _goalsView; }
            set
            {
                _goalsView = value;
                RaisePropertyChanged("GoalsView");
            }
        }

        private int _selectedAssigned;
        public int SelectedAssigned
        {
            get { return _selectedAssigned; }
            set
            {
                _selectedAssigned = value;
                RaisePropertyChanged("SelectedAssigned");
            }
        }

        private int _selectedPool;
        public int SelectedPool
        {
            get { return _selectedPool; }
            set
            {
                _selectedPool = value;
                RaisePropertyChanged("SelectedPool");
            }
        }

        public RelayCommand DateChangedCommand { get; set; }
        public RelayCommand AssignToProjectCommand { get; set; }
        public RelayCommand UnassignFromProjectCommand { get; set; }

        public ProjectViewModel(BusinessProject tabProject)
        {          
            Project = tabProject;
            GoalsView = new GoalsView() { DataContext = new GoalsViewModel(Project) };

            SetRelayCommandHandlers();

             _conditions = new ObservableCollection<string>();
            Conditions.Add((string)Application.Current.Resources["status_0"]);
            Conditions.Add((string)Application.Current.Resources["status_1"]);
            Conditions.Add((string)Application.Current.Resources["status_2"]);
            Conditions.Add((string)Application.Current.Resources["status_3"]);
            Conditions.Add((string)Application.Current.Resources["status_4"]);

            UpdatePersonel();

            MessengerInstance.Register<NotificationMessage>(this, BeingNotifed);
        }

        private void SetRelayCommandHandlers()
        {
            DateChangedCommand = new RelayCommand(() =>
            {
                _ = ConstructedCommentary;
                ConstructedCommentary = "";
            });

            AssignToProjectCommand = new RelayCommand(() =>
            {
                AssignToProject();
            });

            UnassignFromProjectCommand = new RelayCommand(() =>
            {
                UnassignFromProject();
            });
        }

        public void BeingNotifed(NotificationMessage notificationMessage)
        {
            if (notificationMessage.Notification == "Save")
            {
                Model.UpdateProject(Project, (error) =>
                {
                    if (error != null)
                    {
                        MessengerInstance.Send<NotificationMessage<string>>(new NotificationMessage<string>(
                          (string)Application.Current.Resources["Error_database_request"] + "UpdateProject",
                          "Error"));
                    }
                });

                UpdatePersonel();

            }

        }

        public void AssignToProject()
        {
            if ((SelectedPool >= 0) && (Employees.Count > 0))
            {
                
                Model.AssignPersonToProject(Employees[SelectedPool].PersonID, Project.ProjectID, (error) =>
                {
                    if (error != null)
                    {
                        MessengerInstance.Send<NotificationMessage<string>>(new NotificationMessage<string>(
                            (string)Application.Current.Resources["Error_database_request"] + "AssignToProject",
                            "Error"));
                    }
                });

                ProjectEmployees.Add(Employees[SelectedPool]);
                Employees.Remove(Employees[SelectedPool]);

                RaisePropertyChanged("ProjectEmployees");
                RaisePropertyChanged("Employees");

            }
        }

        public void UnassignFromProject()
        {
            if ((SelectedAssigned >= 0) && (ProjectEmployees.Count > 0))
            {
                
                Model.UnassignPersonFromProject(ProjectEmployees[SelectedAssigned].PersonID, Project.ProjectID, (error) =>
                {
                    if (error != null)
                    {
                        MessengerInstance.Send<NotificationMessage<string>>(new NotificationMessage<string>(
                            (string)Application.Current.Resources["Error_database_request"] + "UnssignFromProject",
                            "Error"));
                    }
                });

                Employees.Add(ProjectEmployees[SelectedAssigned]);
                ProjectEmployees.Remove(ProjectEmployees[SelectedAssigned]);

                RaisePropertyChanged("ProjectEmployees");
                RaisePropertyChanged("Employees");
            }

        }


        /// <summary>
        /// Loads personel for this project
        /// </summary>
        public void UpdatePersonel()
        {
            List<BusinessPerson> projectPersonel = new List<BusinessPerson>();

            List<BusinessPerson> residual = new List<BusinessPerson>();

                Model.GetPersonelList(
                 (item, error) =>
                 {
                     if ((error != null) || (item == null))
                     {
                         MessengerInstance.Send<NotificationMessage<string>>(new NotificationMessage<string>(
                                 (string)Application.Current.Resources["Error_database_request"] + "UpdatePersonel",
                                 "Error"));
                     }

                     projectPersonel = item;
                 });


                Model.GetPersonelForProject(Project.ProjectID, ((list, error) =>
                {

                    if ((error != null) && (list.Count == 0))
                    {
                        MessengerInstance.Send<NotificationMessage<string>>(new NotificationMessage<string>(
                            (string)Application.Current.Resources["Error_database_request"] + "GetPersonelForProject",
                            "Error"));
                    }                    

                    foreach (BusinessPerson item in projectPersonel)
                    {
                        var compared = list.Find(i => i.PersonID == item.PersonID);
                        if (compared == null) residual.Add(item);
                    }

                    projectPersonel = list;

                }));
           
            ProjectEmployees = new ObservableCollection<BusinessPerson>(projectPersonel);
            Employees = new ObservableCollection<BusinessPerson>(residual);

            RaisePropertyChanged("ProjectEmployees");
            RaisePropertyChanged("Employees");

        }
    }
}
