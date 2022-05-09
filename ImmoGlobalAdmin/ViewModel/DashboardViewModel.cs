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
        private static DashboardViewModel? _instance = null;
        private static readonly object _padlock = new();

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
                lock (_padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new DashboardViewModel();
                    }
                    return _instance;
                }
            }
        }

        #endregion
    }
}
