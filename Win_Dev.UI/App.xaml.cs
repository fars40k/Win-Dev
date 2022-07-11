using System.Windows;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Threading;
using Win_Dev.UI.ViewModels;
using Win_Dev.Business;
using Win_Dev.Data;
using Win_Dev.UI.Views;

namespace Win_Dev.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private RegistryWorker registryWorker;
        private ApplicationCultures applicationCultures;

        private static ViewModelLocator _viewModelLocator;
        public static ViewModelLocator ViewModelLocator
        {
            get { return _viewModelLocator; }
        }

        static App()
        {
            DispatcherHelper.Initialize();

            _viewModelLocator = new ViewModelLocator();          
        }

        protected void App_Startup(object sender, StartupEventArgs e)
        {
            SimpleIoc.Default.Register<RegistryWorker>();
            SimpleIoc.Default.Register<ApplicationCultures>();

            applicationCultures = SimpleIoc.Default.GetInstance<ApplicationCultures>();
            registryWorker = SimpleIoc.Default.GetInstance<RegistryWorker>();

            // Setting application localisation

            registryWorker.SetAvalableCultures(applicationCultures.Cultures);
            string storedLangSelection = registryWorker.ReadLanguageRegistryEntry();
            applicationCultures.LocalisationDictionary = new ResourceDictionary();
            applicationCultures.LocalisationDictionary.Source =
            applicationCultures.MapCultureToResourceUri(storedLangSelection);
            Application.Current.Resources.MergedDictionaries.Add(applicationCultures.LocalisationDictionary);

            // Creating main window

            MainWindow mainWindow = new MainWindow();
            mainWindow.DataContext = ViewModelLocator.Main;
            mainWindow.Resources.MergedDictionaries.Add(applicationCultures.LocalisationDictionary);
            mainWindow.MainWindowInit();
            mainWindow.Show();

        }
    }
}
