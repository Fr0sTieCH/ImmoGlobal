using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
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
            MainMenuViewModel mainMenu = MainMenuViewModel.GetInstance;

            MainWindow = new MainWindow()
            {
                DataContext = MainViewModel.GetInstance
            };

            MainWindow.Show();
            SetLanguageDictionary();
            base.OnStartup(e);
        }


        private void SetLanguageDictionary()
        {
            
            switch (Thread.CurrentThread.CurrentCulture.ToString())
            {
                case "en-US":
                    SettingsViewModel.GetInstance.CurrentLanguage = "English";
                    break;
                case "de-CH":
                    SettingsViewModel.GetInstance.CurrentLanguage = "Deutsch";
                    break;
                default:
                    SettingsViewModel.GetInstance.CurrentLanguage = "English";
                    break;
            }
        }
    }
}

