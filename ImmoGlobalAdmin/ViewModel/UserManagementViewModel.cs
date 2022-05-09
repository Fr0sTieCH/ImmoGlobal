using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoGlobalAdmin.ViewModel
{
    internal class UserManagementViewModel:BaseViewModel
    {
        #region Singleton
        private static UserManagementViewModel? _instance = null;
        private static readonly object _padlock = new();

        public UserManagementViewModel()
        {
        }

        /// <summary>
        /// returns instance of class UserManagementViewModel
        /// </summary>
        public static UserManagementViewModel GetInstance
        {
            get
            {
                lock (_padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new UserManagementViewModel();
                    }
                    return _instance;
                }
            }
        }

        #endregion
    }
}
