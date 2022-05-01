using ImmoGlobalAdmin.Commands;
using ImmoGlobalAdmin.MainClasses;
using ImmoGlobalAdmin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImmoGlobalAdmin.ViewModel
{
    internal class MainViewModel : BaseViewModel
    {
        private BaseViewModel _selectedViewModel;
        private BaseViewModel activeMainMenuViewModel;
        private BaseViewModel? dialogView;
        private User? loggedInUser;

        private bool showDialog;

        #region Singleton
        private static MainViewModel? instance = null;
        private static readonly object padlock = new();

        public MainViewModel()
        {
            //For testing
            loggedInUser = DataAccessLayer.GetInstance.GetUserByName("TestAnna");
            if(loggedInUser == null)
            {
               DialogViewModel= EFTestingViewModel.GetInstance;
            }
            MainMenuViewModel mainMenu = MainMenuViewModel.GetInstance;
            _selectedViewModel = mainMenu;
            activeMainMenuViewModel = mainMenu;
        }

        /// <summary>
        /// returns instance of class HomeViewModel
        /// </summary>
        public static MainViewModel GetInstance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new MainViewModel();
                    }
                    return instance;
                }
            }
        }

        #endregion

        public User LoggedInUser => loggedInUser;
        public bool InMainMenu => _selectedViewModel == activeMainMenuViewModel;


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
                    return showDialog;
                }
                
            }
            set
            {
                showDialog = value;
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

        public BaseViewModel ActiveMainMenuViewModel => activeMainMenuViewModel;

        public BaseViewModel? DialogViewModel
        {
            get
            {
                return dialogView;
            }
            set
            {
                ShowDialog = true;
                dialogView= value;
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
            dialogView = null;

        }

        #endregion
    }
}
