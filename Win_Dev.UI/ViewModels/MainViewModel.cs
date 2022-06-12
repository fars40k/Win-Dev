using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Ioc;
using Win_Dev.Business;
using Microsoft.Practices.ServiceLocation;
using System.Windows;
using System.Linq;
using Win_Dev.Data;
using System.Windows.Media;
using System.Windows.Controls;
using Win_Dev.UI.Views;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Windows.Media.Effects;
using GalaSoft.MvvmLight.Messaging;

namespace Win_Dev.UI.ViewModels
{

    public class MainViewModel : ViewModelBase
    {
        private DatabaseWorker _databaseWorker;
        private DataAccessObject _dataAccessObject;

        public ObservableCollection<string> CulturesCB { get; set; }

        #region BindedProperties

        private UserControl _tabControlArea;
        public UserControl TabControlArea
        {
            get { return _tabControlArea; }
            set
            {
                _tabControlArea = value;
                RaisePropertyChanged("TabControlArea");
            }
        }

        private string _selectedCulture;
        public string SelectedCulture
        {
            get { return _selectedCulture; }
            set
            {
                if (_selectedCulture != value)
                {
                    RegistryWorker.UpdateLanguageRegistryEntry(value);

                    System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                    Application.Current.Shutdown();
                }

                RaisePropertyChanged("SelectedCulture");
            }
        }

        private SolidColorBrush _connectionStatusColour;
        public SolidColorBrush ConnectionStatusColour
        {
            get { return _connectionStatusColour;  }
            set
            {
                _connectionStatusColour = value;
                RaisePropertyChanged("ConnectionStatusColour");
            }
        }

        private Visibility _databaseUpdating;
        public Visibility DatabaseUpdating
        {
            get { return _databaseUpdating; }
            set
            {
                _databaseUpdating = value;
                RaisePropertyChanged("DatabaseUpdating");
            }
        }

        private string _databaseString;
        public string DatabaseString
        {
            get { return _databaseString; }
            set
            {
                _databaseString = value;
                RaisePropertyChanged("DatabaseString");
            }
        }

        private string _userHelpString;

        public string UserHelpString
        {
            get { return _userHelpString; }
            set
            {
                _userHelpString = value;
                RaisePropertyChanged("UserHelpString");
            }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {

            MessengerInstance.Register<NotificationMessage<string>>(this, BeingNotifed);

            UIInit();

            DatabaseEventsSubscription();

        }

        private void BeingNotifed(NotificationMessage<string> obj)
        {
            if (obj.Notification == "Error")
            {
                UserHelpString = obj.Content ?? "missing";
            }
        }

        private void UIInit()
        {

            CulturesCB = new ObservableCollection<string>();
            ApplicationCultures.Cultures.ToList().ForEach(CulturesCB.Add);

            _selectedCulture = RegistryWorker.ReadLanguageRegistryEntry();
            SelectedCulture = _selectedCulture;

            _dataAccessObject = SimpleIoc.Default.GetInstance<DataAccessObject>();
            _databaseWorker = SimpleIoc.Default.GetInstance<DatabaseWorker>();

            DatabaseUpdating = Visibility.Visible;
            ConnectionStatusColour = new SolidColorBrush(Colors.OrangeRed);
            UserHelpString = (string)Application.Current.Resources["Database_Wait"] ?? "missing";

        }


        private void DatabaseEventsSubscription()
        {

            _databaseWorker.StatusChangedEvent += (state) => 
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    if (state)
                    {
                        if (TabControlArea == null)
                        {
                            TabControlArea = new TableView();                          
                        }

                        UserHelpString = "";
                        DatabaseUpdating = Visibility.Hidden;

                    }
                    else
                    {

                        TabControlArea = null;
                        DatabaseUpdating = Visibility.Visible;
                        UserHelpString = (string)Application.Current.Resources["Database_Fail"] ?? "missing";

                    }
                });
            };

            _databaseWorker.TryUpdateEvent += (state) =>
            {
                Task.Factory.StartNew(() =>
                {
                    DatabaseUpdating = Visibility.Visible;          
                    Thread.Sleep(2000);
                    if (state)
                    {
                        DatabaseUpdating = Visibility.Hidden;
                    }
                });
            };

            _databaseWorker.UpdatedDataLoadedEvent += () =>
            {

                MessengerInstance.Send<NotificationMessage>(new NotificationMessage("Update"));

            };

            _databaseWorker.ConnectionInit();
            
        }       

    }
}