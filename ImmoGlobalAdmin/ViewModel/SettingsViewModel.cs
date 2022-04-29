using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoGlobalAdmin.ViewModel
{
    internal class SettingsViewModel:BaseViewModel
    {
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

        #endregion
    }
}

