using ImmoGlobalAdmin.Commands;
using ImmoGlobalAdmin.MainClasses;
using ImmoGlobalAdmin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace ImmoGlobalAdmin.ViewModel
{
    internal class MainViewModel : BaseViewModel
    {
        private BaseViewModel _selectedViewModel;
        private BaseViewModel _activeMainMenuViewModel;
        private BaseViewModel? _dialogView;
        private User? _loggedInUser;
        private string _usernameToLogin;

        private bool _showDialog;

        #region Singleton
        private static MainViewModel? _instance = null;
        private static readonly object _padlock = new();

        public MainViewModel()
        {
            //For testing
            // _loggedInUser = DataAccessLayer.GetInstance.GetUserByName("BeispielAnna");
            //OnPropertyChanged(nameof(LoggedInUser));
            //if(_loggedInUser == null)
            //{
            //   DialogViewModel= EFTestingViewModel.GetInstance;
            //}
            MainMenuViewModel mainMenu = MainMenuViewModel.GetInstance;
            _selectedViewModel = mainMenu;
            _activeMainMenuViewModel = mainMenu;
        }

        /// <summary>
        /// returns instance of class HomeViewModel
        /// </summary>
        public static MainViewModel GetInstance
        {
            get
            {
                lock (_padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new MainViewModel();
                    }
                    return _instance;
                }
            }


        }

        #endregion

        public User? LoggedInUser => _loggedInUser;
        public bool InMainMenu => _selectedViewModel == _activeMainMenuViewModel;


        private string searchString = "";
        public string SearchString
        {
            get { return searchString; }
            set
            {
                searchString = value;
                OnPropertyChanged(nameof(SearchString));
            }
        }

        //EnableDisable Search function
        public bool SearchAllowed => _selectedViewModel is IHasSearchableContent;

        public bool NoUserLoggedIn => !(_loggedInUser != null);

        public string UsernameToLogIn
        {
            get
            {
                return _usernameToLogin;
            }
            set
            {
                _usernameToLogin = value;
                OnPropertyChanged(nameof(_usernameToLogin));
            }
        }

        public bool ShowDialog
        {
            get
            {
                if (DeleteDialogOpen)
                {

                    return false;
                }
                else
                {
                    return _showDialog;
                }

            }
            set
            {
                _showDialog = value;
                OnPropertyChanged(nameof(ShowDialog));
            }
        }

        public BaseViewModel SelectedViewModel
        {
            get
            {
                return _selectedViewModel;
            }
            set
            {
                _selectedViewModel = value;
                OnPropertyChanged(nameof(SelectedViewModel));
                OnPropertyChanged(nameof(InMainMenu));
                OnPropertyChanged(nameof(SearchAllowed));
            }
        }

        public BaseViewModel ActiveMainMenuViewModel => _activeMainMenuViewModel;

        public BaseViewModel? DialogViewModel
        {
            get
            {
                return _dialogView;
            }
            set
            {
                ShowDialog = true;
                _dialogView = value;
                OnPropertyChanged(nameof(DialogViewModel));
            }
        }


        #region ButtonCommands

        #region Delete Dialog Buttons

        public override void DeleteButtonClicked(object obj)
        {

            base.DeleteButtonClicked(obj);
            OnPropertyChanged(nameof(ShowDialog));
        }
        public override void DeleteAcceptButtonClicked(object obj)
        {

            base.DeleteAcceptButtonClicked(obj);
            OnPropertyChanged(nameof(ShowDialog));
        }
        public override void DeleteCancelButtonClicked(object obj)
        {

            base.DeleteCancelButtonClicked(obj);
            OnPropertyChanged(nameof(ShowDialog));
        }


        #endregion
        public ICommand HomeButtonCommand
        {
            get
            {
                return new RelayCommand<object>(HomeButtonClicked);
            }
        }

        private void HomeButtonClicked(object obj)
        {

            SelectedViewModel = MainMenuViewModel.GetInstance;

        }

        public ICommand SearchButtonCommand
        {
            get
            {
                return new RelayCommand<object>(SearchButtonClicked);
            }
        }

        private void SearchButtonClicked(object obj)
        {

            ExecuteSearch(SearchString);

        }

        private void ExecuteSearch(string searchString)
        {
            if (SelectedViewModel is IHasSearchableContent)
            {
                IHasSearchableContent vm = (IHasSearchableContent)SelectedViewModel;
                vm.SearchContent(searchString);
            }
        }

        public ICommand CloseDialogButtonCommand
        {
            get
            {
                return new RelayCommand<object>(CloseDialogButtonClicked);
            }
        }

        private void CloseDialogButtonClicked(object obj)
        {

            ShowDialog = false;
            _dialogView = null;

        }

        public ICommand LoginUserCommand
        {
            get
            {
                return new RelayCommand<object>(LoginUserButtonClicked);
            }
        }

        private void LoginUserButtonClicked(object obj)
        {
            //only for prototype this is not secure and violates the MVVM architecture
            PasswordBox pwb = (PasswordBox)obj;

            if (DataAccessLayer.GetInstance.GetUserByName(UsernameToLogIn) != null)
            {
                User user = DataAccessLayer.GetInstance.GetUserByName(UsernameToLogIn);
                

                if (user.VerifyCredentials(UsernameToLogIn, pwb.Password))
                {
                    _loggedInUser = user;
                    OnPropertyChanged(nameof(NoUserLoggedIn));
                    OnPropertyChanged(nameof(LoggedInUser));
                    MainMenuViewModel.GetInstance.OnLoggedInUserChanged();
                }

            }

            UsernameToLogIn = "";
            pwb.Clear();

        }


        public ICommand LogoutUserCommand
        {
            get
            {
                return new RelayCommand<object>(LogoutUserButtonClicked);
            }
        }

        private void LogoutUserButtonClicked(object obj)
        {
            SelectedViewModel = MainMenuViewModel.GetInstance;
            _loggedInUser = null;
            OnPropertyChanged(nameof(NoUserLoggedIn));
            OnPropertyChanged(nameof(LoggedInUser));
            MainMenuViewModel.GetInstance.OnLoggedInUserChanged();
        }

        #endregion
    }
}
