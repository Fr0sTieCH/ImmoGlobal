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
        private static UserManagementViewModel? instance = null;
        private static readonly object padlock = new();

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
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new UserManagementViewModel();
                    }
                    return instance;
                }
            }
        }

        #endregion
    }
}
