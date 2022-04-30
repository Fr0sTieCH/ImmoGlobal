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
        private User loggedInUser;


        #region Singleton
        private static MainViewModel? instance = null;
        private static readonly object padlock = new();

        public MainViewModel()
        {
            //For testing
            loggedInUser = DataAccessLayer.GetInstance.GetUserByName("TestAnna");
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



        #region ButtonCommands

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

        #endregion
    }
}
