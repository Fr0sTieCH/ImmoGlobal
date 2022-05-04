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
        #endregion
    }
}
