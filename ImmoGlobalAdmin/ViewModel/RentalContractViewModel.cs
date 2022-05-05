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
       private RentalObject rentalObjetToGetContractsFrom;
        private RealEstate realEstateOfRentalObject;
        private RentalContract? selectedRentalContract;
        private RentalObjectViewModel viewModelToUpdate;
        private DateTime terminationDate;

        public RentalContractViewModel(RentalObject rentalObjetToGetContractsFrom,RealEstate realEstateOfRentalObject, RentalObjectViewModel viewModelToUpdate)
        {
            this.RealEstateOfRentalObject = realEstateOfRentalObject;
            this.RentalObjetToGetContractsFrom = rentalObjetToGetContractsFrom;
            this.SelectedRentalContract = rentalObjetToGetContractsFrom.RentalContracts.FirstOrDefault();
            this.TerminationDate = DateTime.Now;
            this.viewModelToUpdate = viewModelToUpdate;
        }

        public RentalObject RentalObjetToGetContractsFrom
        {
            get
            {
                return rentalObjetToGetContractsFrom;
            }

            set
            {
                
                rentalObjetToGetContractsFrom = value;
                OnPropertyChanged(nameof(RentalObjetToGetContractsFrom));
            }
        }

        public RentalContract? SelectedRentalContract
        {
            get
            {
                return selectedRentalContract;
            }
            set
            {
                
                selectedRentalContract = value;
                OnPropertyChanged(nameof(SelectedRentalContract));
                OnPropertyChanged(nameof(AllowedToValidateSelectedContract));
            }
        }

        public RealEstate RealEstateOfRentalObject
        {
            get 
            { 
                return realEstateOfRentalObject; 
            }
            set
            {
                realEstateOfRentalObject = value;
                OnPropertyChanged(nameof (RealEstateOfRentalObject));
            }
        }



        public DateTime TerminationDate
        {
            get 
            { 
                return terminationDate; 
            }
            set
            {
                terminationDate = value;
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
                if (selectedRentalContract.State == ContractState.ValidationPending)
                {
                    if (rentalObjetToGetContractsFrom.ActiveRentalContract != null)
                    {
                        if (rentalObjetToGetContractsFrom.ActiveRentalContract.EndDate != null)
                        {
                            if (rentalObjetToGetContractsFrom.ActiveRentalContract.EndDate <= selectedRentalContract.StartDate)
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
            selectedRentalContract.TerminateContract(TerminationDate);
            DataAccessLayer.GetInstance.SaveChanges();
            OnPropertyChanged(nameof(RentalObjetToGetContractsFrom.RentalContracts));
            OnPropertyChanged(nameof(selectedRentalContract));
            viewModelToUpdate.UpdateRentalObject();
        }

        public ICommand RevertTerminationButtonCommand => new RelayCommand<object>(RevertTerminationButtonClicked);
        private void RevertTerminationButtonClicked(object obj)
        {
            selectedRentalContract.ChangeEndDate = null;
            selectedRentalContract.CheckState();
            DataAccessLayer.GetInstance.SaveChanges();
            OnPropertyChanged(nameof(RentalObjetToGetContractsFrom.RentalContracts));
            OnPropertyChanged(nameof(selectedRentalContract));
            viewModelToUpdate.UpdateRentalObject();
        }

        public ICommand ValidateButtonCommand => new RelayCommand<object>(ValidateButtonClicked);

        private void ValidateButtonClicked(object obj)
        {
            selectedRentalContract.ValidateContract();
            DataAccessLayer.GetInstance.SaveChanges();
            OnPropertyChanged(nameof(RentalObjetToGetContractsFrom.RentalContracts));
            OnPropertyChanged(nameof(selectedRentalContract));
            viewModelToUpdate.UpdateRentalObject();
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
            if (creationMode)
            {

            }
            else
            {
                DataAccessLayer.GetInstance.RestoreValuesFromDB(selectedRentalContract);
            }

            base.CancelEditButtonClicked(obj);
            OnPropertyChanged(nameof(SelectedRentalContract));
            OnPropertyChanged(nameof(RentalObjetToGetContractsFrom));
        }


        protected override void SaveEditButtonClicked(object obj)
        {
            if (creationMode)
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
            SelectedRentalContract = new RentalContract(rentalObjetToGetContractsFrom,DateTime.Now);
            OnPropertyChanged(nameof(SelectedRentalContract));
            base.CreateButtonClicked(obj);
        }

        #endregion
    }
}
