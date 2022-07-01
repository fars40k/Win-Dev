using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;
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

            GetPersonelList();

            _employeesOldHashCode = Employees.GetHashCode();

            SetRelayCommandHandlers();

            MessengerInstance.Register<NotificationMessage>(this, BeingNotifed);
        }

        public void BeingNotifed(NotificationMessage message)
        {

            if (message.Notification == "Save")
            {
                SavePersonelChanges();

                GetPersonelList();
                _employeesOldHashCode = Employees.GetHashCode();
            }
        }

        public void SetRelayCommandHandlers()
        {
            CreatePersonCommand = new RelayCommand(() =>
            {
                App.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.Background,
                new Action(() => this.CreatePerson()));
            });

            DeletePersonCommand = new RelayCommand(() =>
            {
                App.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.Background,
                new Action(() => this.DeletePerson()));
            });

            SelectionChangedCommand = new RelayCommand<BusinessPerson>((person) =>
            {
                SelectedEmployee = person;
            });
        }

        public void CreatePerson()
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
        }

        public void DeletePerson()
        {
            if (SelectedEmployee != null)
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

                Employees.Remove(SelectedEmployee);
                SelectedEmployee = null;
            }
        }

        public void GetPersonelList()
        {

            List<BusinessPerson> result = new List<BusinessPerson>();

            App.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Background,
                new Action(() =>
                {
                    Model.GetPersonelList(
                        (list, error) =>
                        {
                            if ((error != null) || (list == null))
                            {
                                MessengerInstance.Send<NotificationMessage<string>>(new NotificationMessage<string>(
                                    (string)Application.Current.Resources["Error_database_request"] + "UpdatePersonel",
                                    "Error"));
                            }

                            Employees = new ObservableCollection<BusinessPerson>(list);
                        });
                })); 
        }

        public void SavePersonelChanges()
        {
            RaisePropertyChanged("Employees");

            App.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.Background,
                new Action(() => Model.UpdatePersonel(Employees,
               (error) =>
               {
                   if (error != null)
                   {
                       MessengerInstance.Send<NotificationMessage<string>>(new NotificationMessage<string>(
                           (string)Application.Current.Resources["Error_database_request"] + "SavePersonel",
                           "Error"));
                   }


               })));              
        }
    }
}
