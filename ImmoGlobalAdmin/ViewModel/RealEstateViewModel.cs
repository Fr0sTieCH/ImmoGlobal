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
    internal class RealEstateViewModel : BaseViewModel, IHasSearchableContent
    {
        private string _searchString = "";
        private RealEstate? _selectedRealEstate;

        #region Singleton
        private static RealEstateViewModel? _instance = null;
        private static readonly object _padlock = new();

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
                lock (_padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new RealEstateViewModel();
                    }
                    return _instance;
                }
            }
        }

        #endregion

        #region Binding Properties

        public override List<RealEstate> AllRealEstates
        {
            get
            {
                if (_searchString == "" || _searchString == null)
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
            get { return _searchString; }
            set
            {
                _searchString = value;
                OnPropertyChanged(nameof(AllRealEstates));
            }
        }

        public RealEstate? SelectedRealEstate
        {
            get
            {
                if (_selectedRealEstate == null)
                {
                    _selectedRealEstate = AllRealEstates.FirstOrDefault();
                }

                return _selectedRealEstate;
            }
            set
            {
                if (_editMode || DeleteDialogOpen)
                {
                    return;
                }

                _selectedRealEstate = value;
                OnPropertyChanged(nameof(SelectedRealEstate));
            }
        }





        #endregion

        public void SearchContent(string searchString)
        {
            SearchString = searchString;
        }

        #region Button Commands

        #region Selected RealEstate Buttons
        protected override void EditButtonClicked(object obj)
        {
            OnPropertyChanged(nameof(SelectedRealEstate));
            OnPropertyChanged(nameof(AllRealEstates));
            base.EditButtonClicked(obj);
        }

        protected override void CancelEditButtonClicked(object obj)
        {
            base.CancelEditButtonClicked(obj);

            if (_creationMode)
            {
                SelectedRealEstate = null;
            }
            else
            {
                DataAccessLayer.GetInstance.RestoreValuesFromDB(SelectedRealEstate);
                DataAccessLayer.GetInstance.RestoreValuesFromDB(_selectedRealEstate.BaseObject);
                foreach (RentalObject ro in _selectedRealEstate.RentalObjects)
                {
                    DataAccessLayer.GetInstance.RestoreValuesFromDB(ro);
                }
            }

           
            OnPropertyChanged(nameof(AllRealEstates));
            OnPropertyChanged(nameof(SelectedRealEstate));
            
        }


        protected override void SaveEditButtonClicked(object obj)
        {
            if (_creationMode)
            {
                DataAccessLayer.GetInstance.StoreNewRealEstate(SelectedRealEstate);
                SelectedRealEstate = null;
            }
            else
            {
                DataAccessLayer.GetInstance.SaveChanges();
            }

            base.SaveEditButtonClicked(obj);

            OnPropertyChanged(nameof(SelectedRealEstate));
            OnPropertyChanged(nameof(AllRealEstates));

        }

        protected override void CreateButtonClicked(object obj)
        {
            SelectedRealEstate = new RealEstate(true);
            OnPropertyChanged(nameof(SelectedRealEstate));
            base.CreateButtonClicked(obj);
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
            SelectedRealEstate.Delete($"{MainViewModel.GetInstance.LoggedInUser.Username} deleted this RealEstate on {DateTime.Now} with reason: ({(string)obj})");
            DataAccessLayer.GetInstance.SaveChanges();

            base.DeleteAcceptButtonClicked(obj);

            SelectedRealEstate = null;
            OnPropertyChanged(nameof(_selectedRealEstate));
            OnPropertyChanged(nameof(AllRealEstates));
            MainViewModel.GetInstance.DeleteAcceptButtonClicked(obj);

        }

        public override void DeleteCancelButtonClicked(object obj)
        {
            MainViewModel.GetInstance.DeleteCancelButtonClicked(obj);
            base.DeleteCancelButtonClicked(obj);
        }
        #endregion



        public ICommand OpenRentalObjectButtonCommand => new RelayCommand<object>(OpenRentalObjectButtonClicked);
        private void OpenRentalObjectButtonClicked(object obj)
        {
            if (_selectedRealEstate != null) 
            { 
            RentalObjectViewModel rovm = new RentalObjectViewModel((RentalObject)obj,SelectedRealEstate,false);
            MainViewModel.GetInstance.SelectedViewModel = rovm;
            }
        }

        public ICommand CreateRentalObjectButtonCommand => new RelayCommand<object>(CreateRentalObjectButtonClicked);
        private void CreateRentalObjectButtonClicked(object obj)
        {
            if (SelectedRealEstate != null)
            {
                RentalObjectViewModel rovm = new RentalObjectViewModel(SelectedRealEstate.AddRentalObject(), SelectedRealEstate, true);
                MainViewModel.GetInstance.SelectedViewModel = rovm;
            }
        }



        #endregion
    }
}
