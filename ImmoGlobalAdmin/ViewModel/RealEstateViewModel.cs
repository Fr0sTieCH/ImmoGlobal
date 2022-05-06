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
        private string searchString = "";
        private RealEstate? selectedRealEstate;

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

        public override List<RealEstate> AllRealEstates
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
                OnPropertyChanged(nameof(AllRealEstates));
            }
        }

        public RealEstate? SelectedRealEstate
        {
            get
            {
                if (selectedRealEstate == null)
                {
                    selectedRealEstate = AllRealEstates.FirstOrDefault();
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

            base.CancelEditButtonClicked(obj);

            OnPropertyChanged(nameof(SelectedRealEstate));
            OnPropertyChanged(nameof(AllRealEstates));
        }


        protected override void SaveEditButtonClicked(object obj)
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
            OnPropertyChanged(nameof(selectedRealEstate));
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
            if (selectedRealEstate != null) 
            { 
            RentalObjectViewModel rovm = new RentalObjectViewModel((RentalObject)obj,SelectedRealEstate,false);
            MainViewModel.GetInstance.SelectedViewModel = rovm;
            }
        }

        public ICommand CreateRentalObjectButtonCommand => new RelayCommand<object>(CreateRentalObjectButtonClicked);
        private void CreateRentalObjectButtonClicked(object obj)
        {
            if (selectedRealEstate != null)
            {
                RentalObjectViewModel rovm = new RentalObjectViewModel(new RentalObject(selectedRealEstate.BaseObject), SelectedRealEstate, true);
                MainViewModel.GetInstance.SelectedViewModel = rovm;
            }
        }



        #endregion
    }
}
