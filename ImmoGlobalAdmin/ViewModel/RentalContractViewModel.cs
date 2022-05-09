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
    internal class RentalContractViewModel :BaseViewModel
    {
       private RentalObject _rentalObjetToGetContractsFrom;
        private RealEstate _realEstateOfRentalObject;
        private RentalContract? _selectedRentalContract;
        private RentalObjectViewModel _viewModelToUpdate;
        private DateTime _terminationDate;

        public RentalContractViewModel(RentalObject rentalObjetToGetContractsFrom,RealEstate realEstateOfRentalObject, RentalObjectViewModel viewModelToUpdate)
        {
            this.RealEstateOfRentalObject = realEstateOfRentalObject;
            this.RentalObjetToGetContractsFrom = rentalObjetToGetContractsFrom;
            this.SelectedRentalContract = rentalObjetToGetContractsFrom.RentalContracts.FirstOrDefault();
            this.TerminationDate = DateTime.Now;
            this._viewModelToUpdate = viewModelToUpdate;
        }

        public RentalObject RentalObjetToGetContractsFrom
        {
            get
            {
                return _rentalObjetToGetContractsFrom;
            }

            set
            {
                
                _rentalObjetToGetContractsFrom = value;
                OnPropertyChanged(nameof(RentalObjetToGetContractsFrom));
            }
        }

        public List<RentalContract> RentalContractsOfObject => RentalObjetToGetContractsFrom.RentalContracts.ToList();

        public RentalContract? SelectedRentalContract
        {
            get
            {
                return _selectedRentalContract;
            }
            set
            {
                
                _selectedRentalContract = value;
                OnPropertyChanged(nameof(SelectedRentalContract));
                OnPropertyChanged(nameof(AllowedToValidateSelectedContract));
                OnPropertyChanged(nameof(RentalContractSelected));
            }
        }

        public bool RentalContractSelected => _selectedRentalContract != null;

        public RealEstate RealEstateOfRentalObject
        {
            get 
            { 
                return _realEstateOfRentalObject; 
            }
            set
            {
                _realEstateOfRentalObject = value;
                OnPropertyChanged(nameof (RealEstateOfRentalObject));
            }
        }



        public DateTime TerminationDate
        {
            get 
            { 
                return _terminationDate; 
            }
            set
            {
                _terminationDate = value;
                OnPropertyChanged(nameof (TerminationDate));
            }
        }

        /// <summary>
        /// Check if selected contract can be validated 
        /// the rental object either has no active contract or the active contract is running out/terminated and the startdate of the selected contract is after the end date of the selected contract
        /// </summary>
        public bool AllowedToValidateSelectedContract
        {
            get
            {
                if (_selectedRentalContract == null) return false;

                if (_selectedRentalContract.State == ContractState.ValidationPending)
                {
                    if (_rentalObjetToGetContractsFrom.ActiveRentalContract != null)
                    {
                        if (_rentalObjetToGetContractsFrom.ActiveRentalContract.EndDate != null)
                        {
                            if (_rentalObjetToGetContractsFrom.ActiveRentalContract.EndDate <= _selectedRentalContract.StartDate)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }

                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            
        }

        #region ButtonCommands
        public ICommand TerminateButtonCommand => new RelayCommand<object>(TerminateButtonClicked);
        private void TerminateButtonClicked(object obj)
        {
            _selectedRentalContract.TerminateContract(TerminationDate);
            DataAccessLayer.GetInstance.SaveChanges();
            foreach(RentalContract rc in _rentalObjetToGetContractsFrom.RentalContracts)
            {
                rc.CheckState();
            }
            OnPropertyChanged(nameof(RentalObjetToGetContractsFrom));
            OnPropertyChanged(nameof(RentalContractsOfObject));
            OnPropertyChanged(nameof(_selectedRentalContract));
            _viewModelToUpdate.UpdateRentalObject();
        }

        public ICommand RevertTerminationButtonCommand => new RelayCommand<object>(RevertTerminationButtonClicked);
        private void RevertTerminationButtonClicked(object obj)
        {
            _selectedRentalContract.ChangeEndDate = null;
            DataAccessLayer.GetInstance.SaveChanges();
            OnPropertyChanged(nameof(RentalObjetToGetContractsFrom));
            OnPropertyChanged(nameof(RentalContractsOfObject));
            OnPropertyChanged(nameof(_selectedRentalContract));
            _viewModelToUpdate.UpdateRentalObject();
        }

        public ICommand ValidateButtonCommand => new RelayCommand<object>(ValidateButtonClicked);

        private void ValidateButtonClicked(object obj)
        {
            _selectedRentalContract.ValidateContract();
            DataAccessLayer.GetInstance.SaveChanges();
            OnPropertyChanged(nameof(RentalObjetToGetContractsFrom.RentalContracts));
            OnPropertyChanged(nameof(_selectedRentalContract));
            _viewModelToUpdate.UpdateRentalObject();
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
        #endregion

        protected override void CancelEditButtonClicked(object obj)
        {
            if (_creationMode)
            {
                _rentalObjetToGetContractsFrom.RemoveRentalContract(SelectedRentalContract);
            }
            else
            {
                DataAccessLayer.GetInstance.RestoreValuesFromDB(_selectedRentalContract);
            }

            base.CancelEditButtonClicked(obj);
            OnPropertyChanged(nameof(SelectedRentalContract));
            OnPropertyChanged(nameof(RentalObjetToGetContractsFrom));
        }


        protected override void SaveEditButtonClicked(object obj)
        {
            if (_creationMode)
            {
                DataAccessLayer.GetInstance.StoreNewRentalContract(SelectedRentalContract);
            }
            else
            {
                DataAccessLayer.GetInstance.SaveChanges();
            }

            base.SaveEditButtonClicked(obj);
            OnPropertyChanged(nameof(SelectedRentalContract));
            OnPropertyChanged(nameof(RentalObjetToGetContractsFrom));

        }

        protected override void CreateButtonClicked(object obj)
        {
            SelectedRentalContract = _rentalObjetToGetContractsFrom.CreateNewRentalContract();
            OnPropertyChanged(nameof(SelectedRentalContract));
            base.CreateButtonClicked(obj);
        }

        #endregion
    }
}
