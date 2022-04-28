using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ImmoGlobalAdmin.Commands;
using ImmoGlobalAdmin.MainClasses;
using ImmoGlobalAdmin.Model;

namespace ImmoGlobalAdmin.ViewModel
{
    internal class MainMenuViewModel:BaseViewModel
    {

        #region Singleton
        private static MainMenuViewModel? instance = null;
        private static readonly object padlock = new();

        public MainMenuViewModel()
        {
        }

        /// <summary>
        /// returns instance of class HomeViewModel
        /// </summary>
        public static MainMenuViewModel GetInstance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new MainMenuViewModel();
                    }
                    return instance;
                }
            }
        }

        #endregion
        //booleans for enabling buttons, based on permissions of the logged in user
        public bool FinanceEnabled
        {
            get 
            {
                switch (MainViewModel.GetInstance.LoggedInUser.PermissionLevel)
                {
                    case Permissions.Admin:
                        return true;
      
                    case Permissions.FinanceManager:
                        return true;

                    case Permissions.RentalObjectManager:
                        return false;

                        default:
                        return false;
                }
            }

        }
        public bool RealEstateEnabled
        {
            get
            {
                switch (MainViewModel.GetInstance.LoggedInUser.PermissionLevel)
                {
                    case Permissions.Admin:
                        return true;

                    case Permissions.FinanceManager:
                        return false;

                    case Permissions.RentalObjectManager:
                        return false;

                    default:
                        return false;
                }
            }
        }
        public bool personsEnabled
        {
            get
            {
                switch (MainViewModel.GetInstance.LoggedInUser.PermissionLevel)
                {
                    case Permissions.Admin:
                        return true;

                    case Permissions.FinanceManager:
                        return true;

                    case Permissions.RentalObjectManager:
                        return true;

                    default:
                        return false;
                }
            }
        }
        public bool userEnabled
        {
            get
            {
                switch (MainViewModel.GetInstance.LoggedInUser.PermissionLevel)
                {
                    case Permissions.Admin:
                        return true;

                    case Permissions.FinanceManager:
                        return false;

                    case Permissions.RentalObjectManager:
                        return false;

                    default:
                        return false;
                }
            }
        }

        //booleans for highlightin the current view
        public bool RealEstateActive => !( MainViewModel.GetInstance.SelectedViewModel.GetType() == typeof(RealEstateViewModel));

        #region ButtonCommands

        public ICommand RealEstateButtonCommand
        {
            get
            {
                return new RelayCommand<object>(RealEstateButtonClicked);
            }
        }

        private void RealEstateButtonClicked(object obj)
        {

            MainViewModel.GetInstance.SelectedViewModel = RealEstateViewModel.GetInstance;
            OnPropertyChanged(nameof(RealEstateActive));

        }
        #endregion

    }
}
