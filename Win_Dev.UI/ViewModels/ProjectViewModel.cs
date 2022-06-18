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
using System.Windows.Controls;
using Win_Dev.Assets.UserControls;
using Win_Dev.Business;
using Win_Dev.Data;

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

        public ObservableCollection<BusinessPerson> Employees
        {
            get
            {
                ObservableCollection<BusinessPerson> getPersonel = new ObservableCollection<BusinessPerson>();

                Model.GetPersonelList(
                    (list, error) =>
                    {
                        foreach (var item in list)
                        {
                            getPersonel.Add(item);
                        }
                    });

                return getPersonel;

            }
            set
            {              
                RaisePropertyChanged("Employees");
            }
        }

        public ObservableCollection<BusinessPerson> _projectEmployees;
        public ObservableCollection<BusinessPerson> ProjectEmployees
        {
            get
            {
                ObservableCollection<BusinessPerson> getPersonel = new ObservableCollection<BusinessPerson>();

                Model.GetPersonelForProject(Project.ProjectID,
                    (list, error) =>
                    {
                        foreach (var item in list)
                        {
                            getPersonel.Add(item);
                        }
                    });


                return getPersonel;

            }
            set
            {
                ICollection<Person> toProject = new List<Person>();

                foreach (BusinessPerson item in value)
                {
                    toProject.Add(item.Person);
                }

                Project.Personel = toProject.ToList<Person>();
                RaisePropertyChanged("ProjectEmployees");
            }
        }

        #endregion

        public BusinessPerson SelectedAssigned;
        public BusinessPerson SelectedPool; 

        public RelayCommand DateChangedCommand { get; set; }
        public RelayCommand AssignToProjectCommand { get; set; }
        public RelayCommand UnassignFromProjectCommand { get; set; }

        public ProjectViewModel(BusinessProject tabProject)
        {
            Project = tabProject;

            ProjectEmployees = new ObservableCollection<BusinessPerson>();
            
            DateChangedCommand = new RelayCommand(() =>
            {
                _ = ConstructedCommentary;
                ConstructedCommentary = "";
            });

            AssignToProjectCommand = new RelayCommand(() =>
            {
                if (SelectedPool != null)
                {

                    if (!ProjectEmployees.Contains(SelectedPool))
                    {
                        ProjectEmployees.Add(SelectedPool);
                        SelectedAssigned = SelectedPool;
                    }

                }

                Model.AssignPersonToProject(SelectedPool.PersonID, Project.ProjectID, (error) => 
                {
                    if (error != null)
                    {
                        MessengerInstance.Send<NotificationMessage<string>>(new NotificationMessage<string>(
                            (string)Application.Current.Resources["Error_database_request"] + "AssignToProject",
                            "Error"));
                    }
                });
            });

            UnassignFromProjectCommand = new RelayCommand(() =>
            {
                if (SelectedAssigned != null)
                {
                    ProjectEmployees.Remove(SelectedAssigned);

                }

                Model.UnassignPersonToProject(SelectedAssigned.PersonID, Project.ProjectID, (error) =>
                {
                    if (error != null)
                    {
                        MessengerInstance.Send<NotificationMessage<string>>(new NotificationMessage<string>(
                            (string)Application.Current.Resources["Error_database_request"] + "AssignToProject",
                            "Error"));
                    }
                });

            });

             _conditions = new ObservableCollection<string>();
            Conditions.Add((string)Application.Current.Resources["status_0"]);
            Conditions.Add((string)Application.Current.Resources["status_1"]);
            Conditions.Add((string)Application.Current.Resources["status_2"]);
            Conditions.Add((string)Application.Current.Resources["status_3"]);
            Conditions.Add((string)Application.Current.Resources["status_4"]);

            List<BusinessPerson> personel = UpdatePersonel();
            Employees = new ObservableCollection<BusinessPerson>(personel.AsEnumerable<BusinessPerson>());

           // MessengerInstance.Register<NotificationMessage>(this, BeingNotifed);
        }

        public void BeingNotifed(NotificationMessage notificationMessage)
        {
            if (notificationMessage.Notification == "Save")
            {

            }
            else if (notificationMessage.Notification == "Update")
            {
                List<BusinessPerson> allPersonel = UpdatePersonel();

                Model.GetPersonelForProject(Project.ProjectID, ((list, error) =>
                {

                    if ((error != null) || (list == null))
                    {
                        MessengerInstance.Send<NotificationMessage<string>>(new NotificationMessage<string>(
                            (string)Application.Current.Resources["Error_database_request"] + "GetPersonelForProject",
                            "Error"));
                    }

                    ProjectEmployees = new ObservableCollection<BusinessPerson>(list);

                    Employees = new ObservableCollection<BusinessPerson>(allPersonel.Except(list));

                }));


            }

        }

        public List<BusinessPerson> UpdatePersonel()
        {
            List<BusinessPerson> result = new List<BusinessPerson>();

            Model.GetPersonelList(
               (item, error) =>
               {
                   if ((error != null) || (item == null))
                   {
                       MessengerInstance.Send<NotificationMessage<string>>(new NotificationMessage<string>(
                           (string)Application.Current.Resources["Error_database_request"] + "UpdatePersonel",
                           "Error"));
                   }

                   result = item;
               });

            return result;
        }
    }
}
