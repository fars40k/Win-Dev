using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
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

        public List<BusinessGoal> Goals;
        public BusinessGoal SelectedGoal;

         #region Project_properties

        public string ProjectID
        {
            get => SelectedGoal.GoalID.ToString();
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

        public ObservableCollection<BusinessPerson> ProjectEmployees
        {
            get
            {
                ObservableCollection<BusinessPerson> getPersonel = new ObservableCollection<BusinessPerson>();
                /*
                Model.GetPersonelForProject(Project.ProjectID,
                    (list, error) =>
                    {
                        foreach (var item in list)
                        {
                            getPersonel.Add(item);
                        }
                    });
                    */

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

        public ObservableCollection<BusinessPerson> GoalAssigned
        {
            get
            {
                ObservableCollection<BusinessPerson> getGoalAssigned = new ObservableCollection<BusinessPerson>();

                return getGoalAssigned;
            }
            set
            {
                ICollection<Person> toProject = new List<Person>();

                foreach (BusinessPerson item in value)
                {
                    toProject.Add(item.Person);
                }

                Project.Personel = toProject.ToList<Person>();
                RaisePropertyChanged("GoalAssigned");
            }
        }

        public RelayCommand DateChangedCommand { get; set; }
        public RelayCommand AssignToGoalCommand { get; set; }
        public RelayCommand UnassignFromGoalCommand { get; set; }

        public GoalsViewModel(BusinessProject businessProject)
        {
            Project = businessProject;

            _conditions = new ObservableCollection<string>();
            Conditions.Add((string)Application.Current.Resources["status_0"]);
            Conditions.Add((string)Application.Current.Resources["status_1"]);
            Conditions.Add((string)Application.Current.Resources["status_2"]);
            Conditions.Add((string)Application.Current.Resources["status_3"]);
            Conditions.Add((string)Application.Current.Resources["status_4"]);

        }
    }
}
