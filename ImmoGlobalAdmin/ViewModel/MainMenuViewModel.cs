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
        public bool FinanceActive => !(MainViewModel.GetInstance.SelectedViewModel.GetType() == typeof(FinanceViewModel));
        public bool UserManagementActive => !(MainViewModel.GetInstance.SelectedViewModel.GetType() == typeof(UserManagementViewModel));
        public bool PersonActive => !(MainViewModel.GetInstance.SelectedViewModel.GetType() == typeof(PersonViewModel));
        public bool SettingsActive => !(MainViewModel.GetInstance.SelectedViewModel.GetType() == typeof(SettingsViewModel));
        public bool DashboardActive => !(MainViewModel.GetInstance.SelectedViewModel.GetType() == typeof(DashboardViewModel));

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
            OnActiveUserControlChanged();

        }

        public ICommand FinanceButtonCommand
        {
            get
            {
                return new RelayCommand<object>(FinanceButtonClicked);
            }
        }

        private void FinanceButtonClicked(object obj)
        {

            MainViewModel.GetInstance.SelectedViewModel = FinanceViewModel.GetInstance;
            OnActiveUserControlChanged();

        }

        public ICommand UserManagementButtonCommand
        {
            get
            {
                return new RelayCommand<object>(UserManagementButtonClicked);
            }
        }

        private void UserManagementButtonClicked(object obj)
        {

            MainViewModel.GetInstance.SelectedViewModel = UserManagementViewModel.GetInstance;
            OnActiveUserControlChanged();

        }

        public ICommand PersonButtonCommand
        {
            get
            {
                return new RelayCommand<object>(PersonButtonClicked);
            }
        }

        private void PersonButtonClicked(object obj)
        {

            MainViewModel.GetInstance.SelectedViewModel = PersonViewModel.GetInstance;
            OnActiveUserControlChanged();

        }

        public ICommand SettingsButtonCommand
        {
            get
            {
                return new RelayCommand<object>(SettingsButtonClicked);
            }
        }

        private void SettingsButtonClicked(object obj)
        {

            MainViewModel.GetInstance.SelectedViewModel = SettingsViewModel.GetInstance;
            OnActiveUserControlChanged();

        }

        public ICommand DashboardButtonCommand
        {
            get
            {
                return new RelayCommand<object>(DashboardButtonClicked);
            }
        }

        private void DashboardButtonClicked(object obj)
        {

            MainViewModel.GetInstance.SelectedViewModel = DashboardViewModel.GetInstance;
            OnActiveUserControlChanged();

        }

        #endregion

        /// <summary>
        /// Updates all booleans for enabling/disabling the main menu buttons based on the current active viewmodel
        /// </summary>
        private void OnActiveUserControlChanged()
        {
            OnPropertyChanged(nameof(RealEstateActive));
            OnPropertyChanged(nameof(FinanceActive));
            OnPropertyChanged(nameof(UserManagementActive));
            OnPropertyChanged(nameof(PersonActive));
            OnPropertyChanged(nameof(SettingsActive));
            OnPropertyChanged(nameof(DashboardActive));
        }

    }
}
