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

            // Setting application localisation

            RegistryWorker.AvalableCultures = ApplicationCultures.Cultures;
            string storedLangSelection = RegistryWorker.ReadLanguageRegistryEntry();
            ApplicationCultures.LocalisationDictionary = new ResourceDictionary();
            ApplicationCultures.LocalisationDictionary.Source =
            ApplicationCultures.MapCultureToResourceUri(storedLangSelection);
            Application.Current.Resources.MergedDictionaries.Add(ApplicationCultures.LocalisationDictionary);

            // Creating main window

            MainWindow mainWindow = new MainWindow();
            mainWindow.DataContext = ViewModelLocator.Main;
            mainWindow.Resources.MergedDictionaries.Add(ApplicationCultures.LocalisationDictionary);
            mainWindow.MainWindowInit();
            mainWindow.Show();

        }
    }
}
