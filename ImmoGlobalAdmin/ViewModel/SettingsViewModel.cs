﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ImmoGlobalAdmin.ViewModel
{
    internal class SettingsViewModel:BaseViewModel
    {
        private LanguageSetting currentLanguage;

        #region Singleton
        private static SettingsViewModel? instance = null;
        private static readonly object padlock = new();


        public SettingsViewModel()
        {
        }

        /// <summary>
        /// returns instance of class SettingsViewModel
        /// </summary>
        public static SettingsViewModel GetInstance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new SettingsViewModel();
                    }
                    return instance;
                }
            }
        }

       public String CurrentLanguage
       {
            get 
            {
                if (currentLanguage == null)
                {
                    currentLanguage = LanguageSetting.English;
                }
                return Enum.GetName(currentLanguage); 
            }
            set 
            {

                currentLanguage = Enum.Parse<LanguageSetting>(value) ;
                SetLanguage();
                OnPropertyChanged();
            }

       }
        public string[] Languages => Enum.GetNames(typeof(LanguageSetting));


        private void SetLanguage()
        {
            if (App.Current.Resources.MergedDictionaries.Count >= 3)
            {
                App.Current.Resources.MergedDictionaries.RemoveAt(2);
            }
            
            ResourceDictionary dict = new ResourceDictionary();

            switch (currentLanguage)
            {
                case LanguageSetting.English:
                    dict.Source = new Uri("..\\Resources\\StringResources.xaml",
                                 UriKind.Relative);
                    break;
                case LanguageSetting.Deutsch:
                    dict.Source = new Uri("..\\Resources\\StringResources.de-CH.xaml",
                                       UriKind.Relative);
                    break;
                default:
                    dict.Source = new Uri("..\\Resources\\StringResources.xaml",
                                     UriKind.Relative);
                    break;
            }

            App.Current.Resources.MergedDictionaries.Add(dict);
        }

    
    #endregion
    }

    enum LanguageSetting
    {
        Deutsch,
        English
    }
}

