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

        public RelayCommand CreatePersonCommand { get; set; }
        public RelayCommand DeletePersonCommand { get; set; }
        public RelayCommand<BusinessPerson> SelectionChangedCommand { get; set; }

        private int _employeesOldHashCode;

        public PersonelViewModel(DatabaseWorker dataAccessObject)
        {
            _employees = new ObservableCollection<BusinessPerson>();

            Employees = new ObservableCollection<BusinessPerson>(GetPersonelList());

            _employeesOldHashCode = Employees.GetHashCode();

            SetRelayCommandHandlers();

            MessengerInstance.Register<NotificationMessage>(this, BeingNotifed);
        }

        public void BeingNotifed(NotificationMessage message)
        {

            if (message.Notification == "Save")
            {
                SavePersonelChanges();

                Employees = new ObservableCollection<BusinessPerson>(GetPersonelList());
                _employeesOldHashCode = Employees.GetHashCode();
            }
        }

        public void SetRelayCommandHandlers()
        {
            CreatePersonCommand = new RelayCommand(() =>
            {
                Model.CreatePerson((item, error) =>
                {
                    if (error != null)
                    {

                        MessengerInstance.Send<NotificationMessage<string>>(new NotificationMessage<string>(
                               error + " CreatePerson",
                               "Error"));
                    }

                    Employees.Add(item);
                    SelectedEmployee = item;

                });

            });

            DeletePersonCommand = new RelayCommand(() =>
            {
                Model.DeletePerson(SelectedEmployee,
                    (error) =>
                    {

                        if (error != null)
                        {
                            MessengerInstance.Send<NotificationMessage<string>>(new NotificationMessage<string>(
                               error + " DeletePerson",
                               "Error"));
                        }

                    });

                if (SelectedEmployee != null) Employees.Remove(SelectedEmployee);
                SelectedEmployee = null;

            });

            SelectionChangedCommand = new RelayCommand<BusinessPerson>((person) =>
            {

                SelectedEmployee = person;

            });
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
            RaisePropertyChanged("Employees");

            Model.UpdatePersonel(Employees,
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
