using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Win_Dev.Business;
using Win_Dev.Data;

namespace Win_Dev.UI.ViewModels
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>    
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {  

                SimpleIoc.Default.Register<DataAccessObject>();
                SimpleIoc.Default.Register<DatabaseWorker>();
            }
            else
            {

                SimpleIoc.Default.Register<DataAccessObject>();
                SimpleIoc.Default.Register<DatabaseWorker>();
            }

            SimpleIoc.Default.Register<BusinessModel>();

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<TableViewModel>();
            SimpleIoc.Default.Register<PersonelViewModel>();
            SimpleIoc.Default.Register<ProjectViewModel>();

            // Setting dependencies

            DataAccessObject dataAccessObject = ServiceLocator.Current.GetInstance<DataAccessObject>();
            DatabaseWorker databaseWorker = ServiceLocator.Current.GetInstance<DatabaseWorker>();
            BusinessModel businessModel = ServiceLocator.Current.GetInstance<BusinessModel>();

            databaseWorker.DatabaseWorkerInit(dataAccessObject);
            businessModel.BusinessModelInit(databaseWorker);
        }

        /// <summary>
        /// Gets the Main property.
        /// </summary>
        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public TableViewModel Table
        {
            get
            {
                return ServiceLocator.Current.GetInstance<TableViewModel>();
            }
        }

        public PersonelViewModel Personel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PersonelViewModel>();
            }
        }     


        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }
    }
}