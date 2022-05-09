using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ImmoGlobalAdmin.ViewModel
{
    internal class SettingsViewModel : BaseViewModel
    {
        private LanguageSetting _currentLanguage;

        #region Singleton
        private static SettingsViewModel? _instance = null;
        private static readonly object _padlock = new();


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
                lock (_padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new SettingsViewModel();
                    }
                    return _instance;
                }
            }
        }




        #endregion

        public String CurrentLanguage
        {
            get
            {
                if (_currentLanguage == null)
                {
                    _currentLanguage = LanguageSetting.English;
                }
                return Enum.GetName(_currentLanguage);
            }
            set
            {

                _currentLanguage = Enum.Parse<LanguageSetting>(value);
                SetLanguage();
                OnPropertyChanged();
            }

        }
        public string[] Languages => Enum.GetNames(typeof(LanguageSetting));


        private void SetLanguage()
        {
            if (App.Current.Resources.MergedDictionaries.Count >= 4)
            {
                App.Current.Resources.MergedDictionaries.RemoveAt(2);
                App.Current.Resources.MergedDictionaries.RemoveAt(2);
            }

            ResourceDictionary dict = new ResourceDictionary();


            switch (_currentLanguage)
            {
                case LanguageSetting.English:
                    dict.Source = new Uri("..\\Resources\\StringResources.xaml", UriKind.Relative);
                    break;
                case LanguageSetting.Deutsch:
                    dict.Source = new Uri("..\\Resources\\StringResources.de-CH.xaml", UriKind.Relative);
                    break;
                default:
                    dict.Source = new Uri("..\\Resources\\StringResources.xaml", UriKind.Relative);
                    break;
            }

            App.Current.Resources.MergedDictionaries.Add(dict);

            //load a reversed version to translate Back
            ResourceDictionary dictReversed = new ResourceDictionary();

            foreach (object key in dict.Keys)
            {
                object val = dict[key];

                if (dictReversed[val] == null)
                {
                    dictReversed.Add(val, key);
                }
            }
            App.Current.Resources.MergedDictionaries.Add(dictReversed);
        }
    }

    enum LanguageSetting
    {
        Deutsch,
        English
    }
}

