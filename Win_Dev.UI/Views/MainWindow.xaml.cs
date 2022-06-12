using System;
using System.Windows;
using Win_Dev.UI.ViewModels;
using Win_Dev.Business;
using GalaSoft.MvvmLight.Ioc;
using Win_Dev.Data;

namespace Win_Dev.UI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            Closing += (s, e) => ViewModelLocator.Cleanup();
        }

        public void MainWindowInit()
        {
            InitializeComponent();            
        }
    }
}