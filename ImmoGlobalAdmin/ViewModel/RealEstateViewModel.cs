using ImmoGlobalAdmin.Commands;
using ImmoGlobalAdmin.MainClasses;
using ImmoGlobalAdmin.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImmoGlobalAdmin.ViewModel
{
    internal class RealEstateViewModel:BaseViewModel,IHasSearchableContent
    {
        private string searchString = "";
        private RealEstate? selectedRealEstate;

        private bool editMode;
        private bool creationMode;

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

        #region Binding Properties

        public List<RealEstate> RealEstates
        {
            get
            {
                if (searchString == "" || searchString == null)
                {
                    return DataAccessLayer.GetInstance.GetRealEstatesUnfiltered();
                }
                else
                {
                    //make changes to the search logic here...
                    return DataAccessLayer.GetInstance.GetRealEstatesUnfiltered().Where(x => x.RealEstateName.ToLower().StartsWith(SearchString.ToLower())).ToList();
                }

            }
        }

        public string SearchString
        {
            get { return searchString; }
            set
            {
                searchString = value;
                OnPropertyChanged(nameof(RealEstates));
            }
        }

        public RealEstate? SelectedRealEstate
        {
            get
            {
                if (selectedRealEstate == null)
                {
                    selectedRealEstate = RealEstates.FirstOrDefault();
                }
                return selectedRealEstate;
            }
            set
            {
                if (editMode || DeleteDialogOpen)
                {
                    return;
                }
                selectedRealEstate = value;
                OnPropertyChanged(nameof(selectedRealEstate));
            }
        }

    
        public List<Person> AllPersons => DataAccessLayer.GetInstance.GetPersonsUnfiltered();
        public List<BankAccount> AllBankAccounts => DataAccessLayer.GetInstance.GetBankAccountsUnfiltered();

        public bool EditMode => editMode;
        public bool EditModeInverted => !editMode;

        #endregion

        public void SearchContent(string searchString)
        {
            SearchString = searchString;
        }

        #region Button Commands

        #region Selected RealEstate Buttons
        public ICommand EditButtonCommand
        {
            get
            {
                return new RelayCommand<object>(EditButtonClicked);
            }
        }

        private void EditButtonClicked(object obj)
        {
            editMode = true;
            OnPropertyChanged(nameof(SelectedRealEstate));
            OnPropertyChanged(nameof(RealEstates));
            OnPropertyChanged(nameof(EditMode));
            OnPropertyChanged(nameof(EditModeInverted));
        }

        public ICommand CancelEditButtonCommand
        {
            get
            {
                return new RelayCommand<object>(CancelEditButtonClicked);
            }
        }

        private void CancelEditButtonClicked(object obj)
        {
            if (creationMode)
            {
                SelectedRealEstate = null;

            }
            else
            {
                DataAccessLayer.GetInstance.RestoreValuesFromDB(SelectedRealEstate);
                DataAccessLayer.GetInstance.RestoreValuesFromDB(selectedRealEstate.BaseObject);
                foreach (RentalObject ro in selectedRealEstate.RentalObjects)
                {
                    DataAccessLayer.GetInstance.RestoreValuesFromDB(ro);
                }
            }

            editMode = false;
            creationMode = false;
            OnPropertyChanged(nameof(SelectedRealEstate));
            OnPropertyChanged(nameof(RealEstates));
            OnPropertyChanged(nameof(EditMode));
            OnPropertyChanged(nameof(EditModeInverted));

        }

        public ICommand SaveEditButtonCommand
        {
            get
            {
                return new RelayCommand<object>(SaveEditButtonClicked);
            }
        }

        private void SaveEditButtonClicked(object obj)
        {
            if (creationMode)
            {
                DataAccessLayer.GetInstance.StoreNewRealEstate(SelectedRealEstate);
                SelectedRealEstate = null;
            }
            else
            {
                DataAccessLayer.GetInstance.SaveChanges();

            }

            creationMode = false;
            editMode = false;
            OnPropertyChanged(nameof(SelectedRealEstate));
            OnPropertyChanged(nameof(RealEstates));
            OnPropertyChanged(nameof(EditMode));
            OnPropertyChanged(nameof(EditModeInverted));

        }


        #endregion

        #region Delete Dialog Button Overrides
        public override void DeleteButtonClicked(object obj)
        {
            MainViewModel.GetInstance.DeleteButtonClicked(obj);
            base.DeleteButtonClicked(obj);
        }
        public override void DeleteAcceptButtonClicked(object obj)
        {
            SelectedRealEstate.Delete($"{MainViewModel.GetInstance.LoggedInUser.Username} deleted this RealEstate on {DateTime.Now}");
            DataAccessLayer.GetInstance.SaveChanges();
            SelectedRealEstate=null;
            OnPropertyChanged(nameof(selectedRealEstate));
            OnPropertyChanged(nameof(RealEstates));
            MainViewModel.GetInstance.DeleteAcceptButtonClicked(obj);
            base.DeleteAcceptButtonClicked(obj);    
        }

        public override void DeleteCancelButtonClicked(object obj)
        {
            MainViewModel.GetInstance.DeleteCancelButtonClicked(obj);
            base.DeleteCancelButtonClicked(obj);
        }
        #endregion

        public ICommand CreateRealEstateButtonCommand
        {
            get
            {
                return new RelayCommand<object>(CreateRealEstateButtonClicked);
            }
        }

        private void CreateRealEstateButtonClicked(object obj)
        {
            SelectedRealEstate = new RealEstate(true);

            OnPropertyChanged(nameof(SelectedRealEstate));
            editMode = true;
            creationMode = true;
            OnPropertyChanged(nameof(EditMode));
            OnPropertyChanged(nameof(EditModeInverted));
        }

        public ICommand OpenRentalObjectButtonCommand
        {
            get
            {
                return new RelayCommand<object>(OpenRentalObjectButtonClicked);
            }
        }

        private void OpenRentalObjectButtonClicked(object obj)
        {
            RentalObjectViewModel.GetInstance.RentalObjectToDisplay = (RentalObject)obj;
            MainViewModel.GetInstance.DialogViewModel = RentalObjectViewModel.GetInstance;
        }



        #endregion
    }
}
