using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoGlobalAdmin.ViewModel
{
    internal class DashboardViewModel:BaseViewModel
    {
        #region Singleton
        private static DashboardViewModel? instance = null;
        private static readonly object padlock = new();

        public DashboardViewModel()
        {
        }

        /// <summary>
        /// returns instance of class DashboardViewModel
        /// </summary>
        public static DashboardViewModel GetInstance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new DashboardViewModel();
                    }
                    return instance;
                }
            }
        }

        #endregion
    }
}
