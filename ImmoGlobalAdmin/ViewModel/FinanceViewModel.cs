using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoGlobalAdmin.ViewModel
{
    internal class FinanceViewModel:BaseViewModel
    {
        #region Singleton
        private static FinanceViewModel? instance = null;
        private static readonly object padlock = new();

        public FinanceViewModel()
        {
        }

        /// <summary>
        /// returns instance of class FinanceViewModel
        /// </summary>
        public static FinanceViewModel GetInstance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new FinanceViewModel();
                    }
                    return instance;
                }
            }
        }

        #endregion
    }
}
