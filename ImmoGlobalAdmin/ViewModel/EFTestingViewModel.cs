using ImmoGlobalAdmin.Commands;
using ImmoGlobalAdmin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ImmoGlobalAdmin.ViewModel
{
    internal class EFTestingViewModel:BaseViewModel
    {

        #region Singleton
        private static EFTestingViewModel? instance = null;
        private static readonly object padlock = new();

        protected EFTestingViewModel()
        {
      
        }

        /// <summary>
        /// returns instance of class HomeViewModel
        /// </summary>
        public static EFTestingViewModel GetInstance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new EFTestingViewModel();
                    }
                    return instance;
                }
            }
        }
        #endregion


        public ICommand TestButtonCommand
        {
            get
            {
                return new RelayCommand<object>(TestButtonClicked);
            }
        }

        private void TestButtonClicked(object obj)
        {
            using (var client = new ImmoGlobalContext())
            {
                client.Database.EnsureCreated();
            }

            ImmoGlobalContext db = new ImmoGlobalContext();
           

            DataAccessLayer dal = new DataAccessLayer(db);
            dal.UpdateDB();
        }
    }
}

