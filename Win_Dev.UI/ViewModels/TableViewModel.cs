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
using System.Windows.Controls;
using System.Windows.Media;
using System.Drawing;
using System.Windows;
using Win_Dev.UI.Views;
using Win_Dev.Data;
using System.Windows.Media.Effects;
using Win_Dev.Business;

namespace Win_Dev.UI.ViewModels
{
    public class TableViewModel : ViewModelBase
    {
        public BusinessModel Model = SimpleIoc.Default.GetInstance<BusinessModel>();

        private TabItem _selectedTab;
        public TabItem SelectedTab
        {
            get { return _selectedTab; }
            set
            {
                _selectedTab = value;
                RaisePropertyChanged("SelectedTab");
            }

        }

        public RelayCommand ProjectCreateCommand { get; set; }
        public RelayCommand ProjectDeleteCommand { get; set; }
        public RelayCommand SaveChangesCommand { get; set; }

        private ObservableCollection<TabItem> _tabs; 
        public ObservableCollection<TabItem> Tabs
        {
            get { return _tabs; }
            set
            {
                _tabs = value;
                RaisePropertyChanged("Tabs");
            }
        }

        public TableViewModel()
        {     

            ProjectCreateCommand = new RelayCommand(() => 
            {
                CreateProject();
            });

            ProjectDeleteCommand = new RelayCommand(() =>
            {
                DeleteProject();
            });

            SaveChangesCommand = new RelayCommand(() =>
            {
                MessengerInstance.Send<NotificationMessage>(new NotificationMessage("Save"));
                
            });

            _tabs = new ObservableCollection<TabItem>();
            Tabs = new ObservableCollection<TabItem>();

            TabItem personelTab = new TabItem();
            personelTab.FontSize = 16;
            personelTab.Foreground = new SolidColorBrush(Colors.OrangeRed);
            personelTab.Background = new SolidColorBrush(Colors.AntiqueWhite);
            personelTab.Header = Application.Current.Resources["Personel"] ?? "missing";
            personelTab.Content = new PersonelView();
            personelTab.Tag = "personel";
            Tabs.Add(personelTab);
            SelectedTab = Tabs.First<TabItem>();

            LoadProjectsChanges();

            MessengerInstance.Register<NotificationMessage>(this, BeingNotifed);

        }

        private void CreateProject()
        {
            BusinessProject project;
            TabItem tabToAdd = new TabItem();

            Model.CreateProject((item, error) =>
            {
                project = item;

                tabToAdd.TabIndex = Tabs.Count;
                tabToAdd.Header = project.Name;
                tabToAdd.Content = new ProjectView() { DataContext = new ProjectViewModel(project) };
                tabToAdd.Tag = project;
                tabToAdd.Background = new SolidColorBrush(Colors.AntiqueWhite);
                Tabs.Add(tabToAdd);

                SelectedTab = tabToAdd;

            });
        }

        private void DeleteProject()
        {

            if (SelectedTab.Tag.ToString() != "personel")
            {
                BusinessProject project = SelectedTab.Tag as BusinessProject; 

                Model.DeleteProject(project.ProjectID, (error) =>
                {
                    if (error != null)
                    {

                        MessengerInstance.Send<NotificationMessage<string>>(new NotificationMessage<string>(
                          (string)Application.Current.Resources["Error_database_request"] + "DeleteProject",
                          "Error"));
                    }
                });

                Tabs.Remove(SelectedTab);
            }
        }

        public void BeingNotifed(NotificationMessage notificationMessage)
        {
            if (notificationMessage.Notification == "Save")
            {
                List<BusinessProject> projectsFromTabs = new List<BusinessProject>();

                foreach (TabItem item in Tabs)
                {
                    if (item.Tag is BusinessProject)  projectsFromTabs.Add((BusinessProject)item.Tag);
                }


                Model.UpdateProjects(projectsFromTabs,(error) =>
                {
                    if (error != null)
                    {
                        MessengerInstance.Send<NotificationMessage<string>>(new NotificationMessage<string>(
                          (string)Application.Current.Resources["Error_database_request"] + "UpdateProjects",
                          "Error"));
                    }
                });
            }

            else if (notificationMessage.Notification == "Update")
            {
               
                LoadProjectsChanges();
            }

        }
         
        public void LoadProjectsChanges()
        {
            Model.GetProjectsList((list, error) =>
            {

                if ((error != null) || (list == null))
                {
                    MessengerInstance.Send<NotificationMessage<string>>(new NotificationMessage<string>(
                        (string)Application.Current.Resources["Error_database_request"] + "UpdateProjects",
                        "Error"));

                }

                TabItem personelTab = Tabs.First<TabItem>();
                
                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    int selectedTabIndex = SelectedTab.TabIndex;

                    Tabs.Clear();
                    Tabs.Add(personelTab);

                    foreach (BusinessProject item in list)
                    {
                        TabItem tabToAdd = new TabItem();

                        tabToAdd.TabIndex = Tabs.Count;
                        tabToAdd.Header = item.Name;
                        tabToAdd.Content = new ProjectView() { DataContext = new ProjectViewModel(item) };
                        tabToAdd.Tag = item;
                        tabToAdd.Background = new SolidColorBrush(Colors.AntiqueWhite);
                        Tabs.Add(tabToAdd);

                    }

                    foreach (TabItem item in Tabs)
                    {
                        if (item.TabIndex == selectedTabIndex) SelectedTab = item;
                    }
                });

            });
            
        }

    }
}
