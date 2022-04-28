using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoGlobalAdmin.ViewModel
{
    internal class RealEstateViewModel:BaseViewModel
    {

        #region Singleton
        private static RealEstateViewModel? instance = null;
        private static readonly object padlock = new();

        public RealEstateViewModel()
        {
        }

        /// <summary>
        /// returns instance of class RealEstateViewModel
        /// </summary>
        public static RealEstateViewModel GetInstance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new RealEstateViewModel();
                    }
                    return instance;
                }
            }
        }

        #endregion
    }
}
