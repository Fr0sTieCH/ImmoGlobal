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
            ResourceDictionary dict = new ResourceDictionary();
            switch (Thread.CurrentThread.CurrentCulture.ToString())
            {
                case "en-US":
                    dict.Source = new Uri("..\\Resources\\StringResources.xaml",
                                  UriKind.Relative);
                    break;
                case "de-CH":
                    dict.Source = new Uri("..\\Resources\\StringResources.de-CH.xaml",
                                       UriKind.Relative);
                    break;
                default:
                    dict.Source = new Uri("..\\Resources\\StringResources.xaml",
                                      UriKind.Relative);
                    break;
            }
            this.Resources.MergedDictionaries.Add(dict);
        }
    }
}

