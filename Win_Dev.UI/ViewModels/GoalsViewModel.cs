using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Win_Dev.Business;
using Win_Dev.Data;

namespace Win_Dev.UI.ViewModels
{
    class GoalsViewModel : ViewModelBase
    {
        public BusinessModel Model = SimpleIoc.Default.GetInstance<BusinessModel>();

        public BusinessProject Project;

        public ObservableCollection<BusinessGoal> _goals; 
        public ObservableCollection<BusinessGoal> Goals
        {
            get => _goals;
            set
            {
                _goals = value;
                RaisePropertyChanged("Goals");
            }
        }

        public BusinessGoal _selectedGoal;
        public BusinessGoal SelectedGoal
        {
            get => _selectedGoal;
            set
            {
                _selectedGoal = value;
                RaisePropertyChanged("GoalID");
                RaisePropertyChanged("GoalName");
                RaisePropertyChanged("Description");
                RaisePropertyChanged("CreationDate");
                RaisePropertyChanged("ExpireDate");
                RaisePropertyChanged("Percentage");
                RaisePropertyChanged("SelectedCondition");
            }

        }
         #region Goal_properties

        public string GoalID
        {
            get
            {
                if (SelectedGoal != null) return SelectedGoal.GoalID.ToString();
                return "";
            }
            set
            {
            
            }
        }
        public string GoalName
        {
            get
            {
                if (SelectedGoal != null) return SelectedGoal.Name;
                return "";
            }
            set
            {
                SelectedGoal.Name = value;
                RaisePropertyChanged("GoalName");
            }
        }
        public string Description
        {
            get
            {
                if (SelectedGoal != null) return SelectedGoal.Description;
                return "";
            }
            set
            {
                SelectedGoal.Description = value;
                RaisePropertyChanged("Description");
            }
        }

        #region Project_dates

        public DateTime CreationDate
        {
            get
            {
                if (SelectedGoal != null) return SelectedGoal.CreationDate;
                return DateTime.MinValue;
            }
            set
            {
                SelectedGoal.CreationDate = value;
                RaisePropertyChanged("CreationDate");
                DateChangedCommand.Execute(this);
            }
        }
        public DateTime ExpireDate
        {
            get
            {
                if (SelectedGoal != null) return SelectedGoal.ExpireDate;
                return DateTime.MinValue;
            }
            set
            {
                SelectedGoal.ExpireDate = value;
                RaisePropertyChanged("ExpireDate");
                DateChangedCommand.Execute(this);
            }
        }
        private string _constructedCommentary
        {
            get
            {
                string obj = "";

                if (SelectedGoal != null)
                {          
                    string subtraction = SelectedGoal.ExpireDate.Subtract(SelectedGoal.CreationDate).TotalDays.ToString();

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

                            obj += Application.Current.Resources["To_completion"] + " " +
                                Math.Ceiling(SelectedGoal.ExpireDate.Subtract(DateTime.Now).TotalDays);

                        }
                        else
                        {

                            obj += Application.Current.Resources["Late_for"] + " " +
                                Math.Ceiling(SelectedGoal.ExpireDate.Subtract(DateTime.Now).TotalDays);
                            obj += " (" + DateTime.Now + ") " + Application.Current.Resources["Days"] + ". ";
                        }
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
            get
            {
                if (SelectedGoal != null) return SelectedGoal.Percentage;
                return 0;
            }
            set
            {
                SelectedGoal.Percentage = value;
                RaisePropertyChanged("Persentage");
            }
        }
        public int SelectedCondition
        {
            get
            {
                if (SelectedGoal != null) return SelectedGoal.StatusKey;
                return 0;
            }
            set
            {
                SelectedGoal.StatusKey = value;
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

        #endregion

        private ObservableCollection<BusinessPerson> _projectAssigned;       
        public ObservableCollection<BusinessPerson> ProjectAssigned
        {
            get => _projectAssigned;
            set
            {
                _projectAssigned = value;
                RaisePropertyChanged("ProjectAssigned");
            }

        }

        private ObservableCollection<BusinessPerson> _goalAssigned;
        public ObservableCollection<BusinessPerson> GoalAssigned
        {
            get => _goalAssigned;
            set
            {
                _goalAssigned = value;
                RaisePropertyChanged("GoalAssigned");
            }
        }

        public int _selectedPersonGoal;
        public int SelectedPersonGoal
        {
            get { return _selectedPersonGoal; }
            set
            {
                _selectedPersonGoal = value;
                RaisePropertyChanged("SelectedPersonGoal");
            }
        }

        public int _selectedPersonProject;
        public int SelectedPersonProject
        {
            get { return _selectedPersonProject; }
            set
            {
                _selectedPersonProject = value;
                RaisePropertyChanged("SelectedPersonProject");
            }
        }

        public RelayCommand CreateGoalCommand { get; set; }
        public RelayCommand DeleteGoalCommand { get; set; }
        public RelayCommand<BusinessGoal> SelectionChangedCommand { get; set; }
        public RelayCommand DateChangedCommand { get; set; }
        public RelayCommand AssignToGoalCommand { get; set; }
        public RelayCommand UnassignFromGoalCommand { get; set; }

        public GoalsViewModel(BusinessProject containingProject)
        {
            Project = containingProject;

            ProjectAssigned = new ObservableCollection<BusinessPerson>();
            GoalAssigned = new ObservableCollection<BusinessPerson>();

            SetRelayCommandHandlers();

            UpdateGoals();

            _conditions = new ObservableCollection<string>();
            Conditions.Add((string)Application.Current.Resources["status_0"]);
            Conditions.Add((string)Application.Current.Resources["status_1"]);
            Conditions.Add((string)Application.Current.Resources["status_2"]);
            Conditions.Add((string)Application.Current.Resources["status_3"]);
            Conditions.Add((string)Application.Current.Resources["status_4"]);

            MessengerInstance.Register<NotificationMessage>(this, BeingNotifed);
        }

        public void BeingNotifed(NotificationMessage notificationMessage)
        {
            if (notificationMessage.Notification == "Save")
            {

            }

            else if (notificationMessage.Notification == "Update")
            {
                
            }

        }

        private void SetRelayCommandHandlers()
        {
            CreateGoalCommand = new RelayCommand(() =>
            {

                Model.CreateGoal(Project.ProjectID, (item, error) =>
                {
                    if (error != null)
                    {

                        MessengerInstance.Send<NotificationMessage<string>>(new NotificationMessage<string>(
                               error + " CreatePerson",
                               "Error"));
                    }

                    Goals.Add(item);
                    SelectedGoal = item;

                });

                MessengerInstance.Send<NotificationMessage>(new NotificationMessage("Update"));

            });

            DeleteGoalCommand = new RelayCommand(() => 
            {
                Model.DeleteGoal(SelectedGoal, (error) =>
                 {
                     if (error != null)
                     {
                         MessengerInstance.Send<NotificationMessage<string>>(new NotificationMessage<string>(
                             (string)Application.Current.Resources["Error_database_request"] + "DeleteGoal",
                             "Error"));
                     }

                     Goals.Remove(SelectedGoal);
                     
                 });
            });

            AssignToGoalCommand = new RelayCommand(() =>
            {
                if ((ProjectAssigned.Count() > 0) && (SelectedPersonProject >= 0) && 
                                                            (ProjectAssigned[SelectedPersonProject] != null))
                {

                    Model.AssignPersonToProject(ProjectAssigned[SelectedPersonProject].PersonID, Project.ProjectID, (error) =>
                    {
                        if (error != null)
                        {
                            MessengerInstance.Send<NotificationMessage<string>>(new NotificationMessage<string>(
                                (string)Application.Current.Resources["Error_database_request"] + "AssignToProject",
                                "Error"));
                        }
                    });

                    GoalAssigned.Add(ProjectAssigned[SelectedPersonProject]);
                    ProjectAssigned.Remove(ProjectAssigned[SelectedPersonProject]);

                }


            });

            UnassignFromGoalCommand = new RelayCommand(() =>
            {
                if ((GoalAssigned.Count() > 0) && (SelectedPersonGoal >= 0) && (GoalAssigned[SelectedPersonGoal] != null))
                {

                    Model.UnassignPersonToProject(GoalAssigned[SelectedPersonGoal].PersonID, Project.ProjectID, (error) =>
                    {
                        if (error != null)
                        {
                            MessengerInstance.Send<NotificationMessage<string>>(new NotificationMessage<string>(
                                (string)Application.Current.Resources["Error_database_request"] + "UnssignFromProject",
                                "Error"));
                        }
                    });

                    ProjectAssigned.Add(GoalAssigned[SelectedPersonGoal]);
                    GoalAssigned.Remove(GoalAssigned[SelectedPersonGoal]);

                }
            });

            SelectionChangedCommand = new RelayCommand<BusinessGoal>((goal) =>
            {
                SelectedGoal = goal;
                if (SelectedGoal != null) UpdatePersonel(SelectedGoal.GoalID);             
            });

            DateChangedCommand = new RelayCommand(() =>
            {
                _ = ConstructedCommentary;
                ConstructedCommentary = "";
            });

        }

        public void UpdateGoals()
        {
            Model.GetGoalsListForProject(Project.ProjectID, (list, error) =>
            {
                if (error != null)
                {

                    MessengerInstance.Send<NotificationMessage<string>>(new NotificationMessage<string>(
                      (string)Application.Current.Resources["Error_database_request"] + "UpdateGoals",
                      "Error"));
                }

                Goals = new ObservableCollection<BusinessGoal>(list);                
            });
     
        }

        public void UpdatePersonel(Guid GoalID)
        {
            List<BusinessPerson> goalPersonel = new List<BusinessPerson>();

            Model.GetPersonelForGoal(GoalID, (list, error) =>
            {
                if (error != null)
                {

                    MessengerInstance.Send<NotificationMessage<string>>(new NotificationMessage<string>(
                      (string)Application.Current.Resources["Error_database_request"] + "InGoal:GetPersonelForGoal",
                      "Error"));
                }

                GoalAssigned = new ObservableCollection<BusinessPerson>(list);
            });

            Model.GetPersonelForProject(Project.ProjectID, (list, error) =>
            {

                if ((error != null) || (list == null))
                {
                    MessengerInstance.Send<NotificationMessage<string>>(new NotificationMessage<string>(
                        (string)Application.Current.Resources["Error_database_request"] + "InGoal:GetPersonelForProject",
                        "Error"));
                }

                ProjectAssigned = new ObservableCollection<BusinessPerson>(list);
            });
        
        }
    }
}
