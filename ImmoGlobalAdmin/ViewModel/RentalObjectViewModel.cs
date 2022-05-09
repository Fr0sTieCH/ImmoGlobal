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
    internal class RentalObjectViewModel : BaseViewModel
    {
        private RentalObject _rentalObjectToDisplay;
        private RealEstate _realEstateOfRentalObject;
        private BaseViewModel? _secondaryViewModel;


        public RentalObjectViewModel(RentalObject rentalObjectToDisplay, RealEstate realEstateOfRentalObject,bool creationMode)
        {
            this.RentalObjectToDisplay = rentalObjectToDisplay;
            this.RealEstateOfRentalObject = realEstateOfRentalObject;
            this._creationMode = creationMode;
            this._editMode = creationMode;

        }

        #region Binding Properties
        public RentalObject RentalObjectToDisplay
        {
            get
            {
                return _rentalObjectToDisplay;
            }
            set
            {
                _rentalObjectToDisplay = value;
                OnPropertyChanged(nameof(RentalObjectToDisplay));
            }
        }

        public RealEstate RealEstateOfRentalObject
        {
            get
            {
                return _realEstateOfRentalObject;
            }
            set
            {
                _realEstateOfRentalObject = value;
                OnPropertyChanged(nameof(RealEstateOfRentalObject));
            }
        }

        public BaseViewModel? SecondaryViewModel
        {
            get
            {
                return _secondaryViewModel;
            }
            set
            {

                _secondaryViewModel = value;
                OnPropertyChanged(nameof(SecondaryViewModel));
            }
        }




        #endregion

        public void UpdateRentalObject()
        {
            OnPropertyChanged(nameof(RentalObjectToDisplay));
        }

        #region Delete Dialog Buttons
        public override void DeleteButtonClicked(object obj)
        {
            MainViewModel.GetInstance.DeleteButtonClicked(obj);
            base.DeleteButtonClicked(obj);
        }
        public override void DeleteAcceptButtonClicked(object obj)
        {
            MainViewModel.GetInstance.DeleteAcceptButtonClicked(obj);
            base.DeleteAcceptButtonClicked(obj);
        }
        public override void DeleteCancelButtonClicked(object obj)
        {
            MainViewModel.GetInstance.DeleteCancelButtonClicked(obj);
            base.DeleteCancelButtonClicked(obj);
        }
        protected override void EditButtonClicked(object obj)
        {
            if (SecondaryViewModel != null) SecondaryViewModel = null;
            base.EditButtonClicked(obj);
        }

        protected override void CancelEditButtonClicked(object obj)
        {
            if (_creationMode)
            {
                _realEstateOfRentalObject.RemoveRentalObject(RentalObjectToDisplay);
                CloseViewClicked("");
            }
            else
            {
                DataAccessLayer.GetInstance.RestoreValuesFromDB(_rentalObjectToDisplay);
            }

            base.CancelEditButtonClicked(obj);
            OnPropertyChanged(nameof(RentalObjectToDisplay));
            OnPropertyChanged(nameof(AllRealEstates));
        }


        protected override void SaveEditButtonClicked(object obj)
        {
            if (_creationMode)
            {
                DataAccessLayer.GetInstance.StoreNewRentalObject(RentalObjectToDisplay);
            }
            else
            {
                DataAccessLayer.GetInstance.SaveChanges();
            }

            base.SaveEditButtonClicked(obj);
            OnPropertyChanged(nameof(AllRealEstates));
            OnPropertyChanged(nameof(RentalObjectToDisplay));

        }

        public ICommand CloseViewCommand => new RelayCommand<object>(CloseViewClicked);

        private void CloseViewClicked(object obj)
        {
            MainViewModel.GetInstance.SelectedViewModel = RealEstateViewModel.GetInstance;
        }

        public ICommand ShowRentalContractsViewCommand => new RelayCommand<object>(ShowRentalContractsViewClicked);
        private void ShowRentalContractsViewClicked(object obj)
        {
            if (_secondaryViewModel !=null)
            {
                if (_secondaryViewModel.GetType() != typeof(RentalContractViewModel))
                {
                    SecondaryViewModel = new RentalContractViewModel(_rentalObjectToDisplay, _realEstateOfRentalObject, this);
                }
                else
                {
                    SecondaryViewModel = null;
                }
            }
            else
            {
                SecondaryViewModel = new RentalContractViewModel(_rentalObjectToDisplay, _realEstateOfRentalObject, this);
            }

        }

        public ICommand ShowTransactionsViewCommand => new RelayCommand<object>(ShowTransactionsViewClicked);
        private void ShowTransactionsViewClicked(object obj)
        {
            if (_secondaryViewModel != null)
            {
                if (_secondaryViewModel.GetType() != typeof(TransactionsViewModel))
                {
                    SecondaryViewModel = new TransactionsViewModel(_rentalObjectToDisplay);
                }
                else
                {
                    SecondaryViewModel = null;
                }
            }
            else
            {
                SecondaryViewModel = new TransactionsViewModel(_rentalObjectToDisplay); 
            }

        }
        #endregion
    }
}
