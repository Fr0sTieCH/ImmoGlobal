using ImmoGlobalAdmin.Commands;
using ImmoGlobalAdmin.Model;
using ImmoGlobalAdmin.MainClasses;
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
            DataAccessLayer.GetInstance.UpdateDB();

            //Create Test Objects

            Person dummyPerson = new Person("Test","Anna","Testweg 1","079 000 00 00","","testanna@gmail.com",null,"This Is A Test Person");

            DataAccessLayer.GetInstance.StoreNewPerson(dummyPerson);

        }
    }
}

