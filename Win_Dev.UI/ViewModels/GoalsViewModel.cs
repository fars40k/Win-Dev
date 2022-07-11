using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Win_Dev.Business;

namespace Win_Dev.UI.ViewModels
{
    class GoalsViewModel : ViewModelBase
    {
        public BusinessModel Model = SimpleIoc.Default.GetInstance<BusinessModel>();

        public BusinessProject Project;

        private ObservableCollection<BusinessGoal> _goals; 
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
                if (SelectedGoal != null) SelectedGoal.Name = value;
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
                if (SelectedGoal != null) SelectedGoal.Description = value;
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
                if (SelectedGoal != null) SelectedGoal.CreationDate = value;
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
                if (SelectedGoal != null) SelectedGoal.ExpireDate = value;
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
                if (SelectedGoal != null) SelectedGoal.Percentage = value;
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
                if (SelectedGoal != null) SelectedGoal.StatusKey = value;
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

        private int _selectedPersonGoal;
        public int SelectedPersonGoal
        {
            get { return _selectedPersonGoal; }
            set
            {
                _selectedPersonGoal = value;
                RaisePropertyChanged("SelectedPersonGoal");
            }
        }

        private int _selectedPersonProject;
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
                Model.UpdateGoals(Goals, (error) =>
                {
                    if (error != null)
                    {
                        MessengerInstance.Send<NotificationMessage<string>>(new NotificationMessage<string>(
                               error + "UpdateGoals",
                               "Error"));
                    }
                });

                UpdateGoals();
            }

        }

        private void SetRelayCommandHandlers()
        {
            CreateGoalCommand = new RelayCommand(() =>
            {
                App.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.Background,
                new Action(() => CreateGoal()));              
            });

            DeleteGoalCommand = new RelayCommand(() => 
            {
                App.Current.Dispatcher.BeginInvoke(
                   DispatcherPriority.Background,
               new Action(() => DeleteGoal()));
            });

            AssignToGoalCommand = new RelayCommand(() =>
            {
                AssignToGoal();
            });

            UnassignFromGoalCommand = new RelayCommand(() =>
            {
                UnassignFromoGoal();
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

        public void CreateGoal()
        {
            App.Current.Dispatcher.BeginInvoke(
               DispatcherPriority.Background,
           new Action(() => Model.CreateGoal(Project.ProjectID, (item, error) =>
            {
                if (error != null)
                {

                    MessengerInstance.Send<NotificationMessage<string>>(new NotificationMessage<string>(
                           error + " CreatePerson",
                           "Error"));
                }

                Goals.Add(item);
                SelectedGoal = item;

            })));

            MessengerInstance.Send<NotificationMessage>(new NotificationMessage("Update"));
        }

        public void DeleteGoal()
        {
            if (SelectedGoal != null)
            {
                App.Current.Dispatcher.BeginInvoke(
                   DispatcherPriority.Background,
               new Action(() => Model.DeleteGoal(SelectedGoal, (error) =>
                {
                    if (error != null)
                    {
                        MessengerInstance.Send<NotificationMessage<string>>(new NotificationMessage<string>(
                            (string)Application.Current.Resources["Error_database_request"] + "DeleteGoal",
                            "Error"));
                    }

                    Goals.Remove(SelectedGoal);
                    SelectedGoal = null;
                })));

            }
        }

        public void AssignToGoal()
        {
            if ((ProjectAssigned.Count() > 0) && (SelectedPersonProject >= 0) &&
               (SelectedPersonProject <= ProjectAssigned.Count()) && (ProjectAssigned[SelectedPersonProject] != null))
            {
             
                Model.AssignPersonToGoal(ProjectAssigned[SelectedPersonProject].PersonID, SelectedGoal.GoalID, (error) =>
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


        }

        public void UnassignFromoGoal()
        {
            if ((GoalAssigned.Count() > 0) && (SelectedPersonGoal >= 0) &&
               (SelectedPersonGoal <= GoalAssigned.Count()) && (GoalAssigned[SelectedPersonGoal] != null))
            {
                Model.UnassignPersonFromGoal(GoalAssigned[SelectedPersonGoal].PersonID, SelectedGoal.GoalID, (error) =>
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
        }


        public void UpdateGoals()
        {
            App.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Background,
            new Action(() =>
            Model.GetGoalsListForProject(Project.ProjectID, (list, error) =>
            {
                if ((error != null) && (list.Count != 0))
                {

                    MessengerInstance.Send<NotificationMessage<string>>(new NotificationMessage<string>(
                      (string)Application.Current.Resources["Error_database_request"] + "UpdateGoals",
                      "Error"));
                }

                Goals = new ObservableCollection<BusinessGoal>(list);                
            })));
     
        }


        /// <summary>
        /// Loads personel for goal in parametre
        /// </summary>
        public void UpdatePersonel(Guid GoalID)
        {
            List<BusinessPerson> projectPersonel = new List<BusinessPerson>();

            Model.GetPersonelForProject(Project.ProjectID, (list, error) =>
            {

                if ((error != null) && (list.Count == 0))
                {
                    MessengerInstance.Send<NotificationMessage<string>>(new NotificationMessage<string>(
                        (string)Application.Current.Resources["Error_database_request"] + "InGoal:GetPersonelForProject",
                        "Error"));
                }

                projectPersonel = list;
            });

            Model.GetPersonelForGoal(GoalID, (list, error) =>
            {
                if ((error != null)&&(list.Count == 0))
                {

                    MessengerInstance.Send<NotificationMessage<string>>(new NotificationMessage<string>(
                      (string)Application.Current.Resources["Error_database_request"] + "InGoal:GetPersonelForGoal",
                      "Error"));
                }

                GoalAssigned = new ObservableCollection<BusinessPerson>(list);

                List<BusinessPerson> residual = new List<BusinessPerson>();

                foreach (BusinessPerson item in projectPersonel)
                {
                    var compared = list.Find(i => i.PersonID == item.PersonID);
                    if (compared == null) residual.Add(item);
                }

                ProjectAssigned = new ObservableCollection<BusinessPerson>(residual);

            });

        }
    }
}
