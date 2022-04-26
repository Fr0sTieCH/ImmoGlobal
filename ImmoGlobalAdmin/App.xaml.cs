using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ImmoGlobalAdmin.ViewModel;

namespace ImmoGlobalAdmin
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            EFTestingViewModel efTestingViewModel = EFTestingViewModel.GetInstance;

            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(efTestingViewModel)
            };

            MainWindow.Show();

            base.OnStartup(e);
        }
    }
}
