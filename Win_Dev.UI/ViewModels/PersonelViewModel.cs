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
    public class PersonelViewModel : ViewModelBase
    {
        public BusinessModel Model = SimpleIoc.Default.GetInstance<BusinessModel>();

        private int _employeesOldHashCode;

        private ObservableCollection<BusinessPerson> _employees;
        public ObservableCollection<BusinessPerson> Employees
        {
            get { return _employees; }
            set
            {
                _employees = value;
                RaisePropertyChanged("Employees");
            }
        }

        private BusinessPerson _selectedEmployee;
        public BusinessPerson SelectedEmployee
        {
            get { return _selectedEmployee; }
            set
            {
                _selectedEmployee = value;
                RaisePropertyChanged("SelectedEmployee");
            }
        }

        public RelayCommand PersonCreateCommand { get; set; }
        public RelayCommand PersonDeleteCommand { get; set; }
        public RelayCommand<BusinessPerson> SelectionChangedCommand { get; set; }


        public PersonelViewModel(DatabaseWorker dataAccessObject)
        {
            _employees = new ObservableCollection<BusinessPerson>();

            Model.GetPersonelList((list,error) =>
            {
                Employees = new ObservableCollection<BusinessPerson>(list);

                MessengerInstance.Send<NotificationMessage<string>>(new NotificationMessage<string>(
                           error + " UpdatePersonel",
                           "Error"));

            });

            _employeesOldHashCode = Employees.GetHashCode();


            PersonCreateCommand = new RelayCommand(() =>
            {
                Model.CreatePerson((item,error) =>
                {
                    MessengerInstance.Send<NotificationMessage<string>>(new NotificationMessage<string>(
                           error + " CreatePerson",
                           "Error"));

                    Employees.Add(item);
                    SelectedEmployee = item;

                });

                MessengerInstance.Send<NotificationMessage>(new NotificationMessage("Update"));

            });

            PersonDeleteCommand = new RelayCommand(() =>
            {

                Model.DeletePerson(SelectedEmployee,
                    (error) =>
                    {
                        if (SelectedEmployee != null) Employees.Remove(SelectedEmployee);

                        

                    });

                SelectedEmployee = null;

            });

            SelectionChangedCommand = new RelayCommand<BusinessPerson>((person) =>
            {

                SelectedEmployee = person;

            });

            MessengerInstance.Register<NotificationMessage>(this, BeingNotifed);
        }

        public void BeingNotifed(NotificationMessage message)
        {
            // If the user has made changes in the persons list it sending changes to database.
            if (message.Notification == "Save")
            {
                SavePersonelChanges();
                GetPersonelList();
                _employeesOldHashCode = Employees.GetHashCode();

            }
            // If this tab has no user changes it updates 

            // TODO fix updating list
            else if (message.Notification == "Update")
            {
                List<BusinessPerson> updatedPersonel = GetPersonelList();

                if (_employeesOldHashCode == updatedPersonel.GetHashCode())
                {
                   
                    Employees = new ObservableCollection<BusinessPerson>(updatedPersonel);
                    _employeesOldHashCode = Employees.GetHashCode();

                }

            }

        }

        public List<BusinessPerson> GetPersonelList()
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

        public void SavePersonelChanges()
        {
            Model.UpdatePersonelList(_employees,
               (error) =>
               {
                   if (error != null)
                   {
                       MessengerInstance.Send<NotificationMessage<string>>(new NotificationMessage<string>(
                           (string)Application.Current.Resources["Error_database_request"] + "SavePersonel",
                           "Error"));
                   }


               });              
        }
    }
}
