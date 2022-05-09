using ImmoGlobalAdmin.MainClasses;
using ImmoGlobalAdmin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ImmoGlobalAdmin.ViewModel
{
    internal class TransactionsViewModel : BaseViewModel, IHasSearchableContent
    {

        private string _searchString = "";
        private Transaction? _selectedTransaction;
        private RealEstate? _realEstateToSelectObjects;//used for choosing a rentalObject on creation
        private RentalObject? _rentalObjectToFilterTransactions = null; //if not null, only the transactions of this specific Object will get loaded

        #region constructors
        public TransactionsViewModel() { }
        public TransactionsViewModel(RentalObject rentalObjectToFilterTransactions)
        {
            this._rentalObjectToFilterTransactions = rentalObjectToFilterTransactions;
        }
        #endregion

        #region Binding Properties
        public override List<Transaction> AllTransactions
        {
            get
            {
                if (_rentalObjectToFilterTransactions == null)//if not null, get only Transactions of the specified rental object
                {
                    if (_searchString == "" || _searchString == null)
                    {
                        return DataAccessLayer.GetInstance.GetTransactionsUnfiltered();
                    }
                    else
                    {
                        //make changes to the search logic here...
                        return DataAccessLayer.GetInstance.GetTransactionsUnfiltered().Where(x => x.IGID.ToLower().StartsWith(SearchString.ToLower())).ToList();
                    }
                }
                else
                {

                    return _rentalObjectToFilterTransactions.Transactions.Where(x=>x.Enabled==true).ToList(); 
                }

            }
        }

        public Transaction? SelectedTransaction
        {
            get
            {
                if (_selectedTransaction == null)
                {
                    _selectedTransaction = AllTransactions.FirstOrDefault();
                }

                return _selectedTransaction;
            }
            set
            {
                if (_editMode || DeleteDialogOpen)
                {
                    return;
                }

                _selectedTransaction = value;
                RealEstateToSelectObjects = GetRealEstateFromTransaction(value);
                
                OnPropertyChanged(nameof(RealEstateToSelectObjects));
                OnPropertyChanged(nameof(ObjectsToSelect));
                OnPropertyChanged(nameof(SelectedObject));
                OnPropertyChanged(nameof(SelectedTransaction));
            }
        }

        public string TranslatedTypeOfSelectedTransaction
        {
            get
            {
                if (_selectedTransaction == null) return "";
                return Application.Current.TryFindResource(SelectedTransaction.SetTypeString) as string ?? SelectedTransaction.SetTypeString;
            }
            set
            {
                if (_selectedTransaction == null) return;
                string convertedString = Application.Current.TryFindResource(value) as string ?? value;
                SelectedTransaction.SetTypeString = convertedString;
            }
        }

        public RealEstate? RealEstateToSelectObjects
        {
            get
            {

                return _realEstateToSelectObjects;
            }
            set
            {
                _realEstateToSelectObjects = value;
                OnPropertyChanged(nameof(RealEstateToSelectObjects));
                OnPropertyChanged(nameof(SelectedObject));
                OnPropertyChanged(nameof(ObjectsToSelect));
                
            }
        }

        public List<RentalObject> ObjectsToSelect
        {
            get
            {
                if (RealEstateToSelectObjects == null)
                {
                    return new List<RentalObject>();
                }
                else
                {
                    List<RentalObject> list;

                    if (RealEstateToSelectObjects.RentalObjects != null)
                    {
                        list = RealEstateToSelectObjects.RentalObjects.ToList();
                    }
                    else
                    {
                        list = new List<RentalObject>();
                    }

                    list.Insert(0,_realEstateToSelectObjects.BaseObject);
                    return list;
                }
            }
        }

        public RentalObject? SelectedObject
        {
            get 
            {
                if (_selectedTransaction == null) return null;
                return SelectedTransaction.RentalObject; 
            }
            set
            {
                SelectedTransaction.SetRentalObject = value;

                if (_creationMode && value!=null)
                {
                    _selectedTransaction.SetBankAccount = value.Account;

                    if (_selectedTransaction.Type == TransactionType.Rent && value.ActiveRentalContract != null)
                    {

                        _selectedTransaction.SetAssociatedPerson = value.ActiveRentalContract.Tenant;
                        _selectedTransaction.SetValue = value.ActiveRentalContract.RentTotal;
                    }
                    OnPropertyChanged(nameof(SelectedTransaction));
                    OnPropertyChanged(nameof(SelectedTransaction.SetAssociatedPerson));
                    OnPropertyChanged(nameof(SelectedTransaction.SetValue));
                }
            }
        }

        public string SearchString
        {
            get { return _searchString; }
            set
            {
                _searchString = value;
                OnPropertyChanged(nameof(AllTransactions));
            }
        }

        public bool ButtonsEnabled => _rentalObjectToFilterTransactions == null;
        #endregion

        #region Button Methods
        protected override void CancelEditButtonClicked(object obj)
        {
            
            base.CancelEditButtonClicked(obj);
            SelectedTransaction = null;
            OnPropertyChanged(nameof(SelectedTransaction));
            OnPropertyChanged(nameof(AllTransactions));
        }

        protected override void SaveEditButtonClicked(object obj)
        {
            if (SelectedTransaction != null)
            {
                SelectedTransaction.Lock();
                if (SelectedTransaction.RentalObject != null)
                {
                    _selectedTransaction.RentalObject.Transactions.Add(_selectedTransaction);
                }
                DataAccessLayer.GetInstance.StoreNewTransaction(SelectedTransaction);
                SelectedTransaction = null;
                _realEstateToSelectObjects = null;
            }

            base.SaveEditButtonClicked(obj);
            OnPropertyChanged(nameof(SelectedTransaction));
            OnPropertyChanged(nameof(AllTransactions));
        }

        protected override void CreateButtonClicked(object obj)
        {
            SelectedTransaction = new Transaction(true);
            SelectedTransaction.SetDateTimeOfTransaction = DateTime.Now;
            OnPropertyChanged(nameof(SelectedTransaction));
            OnPropertyChanged(nameof(TranslatedTypeOfSelectedTransaction));
            base.CreateButtonClicked(obj);
        }

        #region Delete Dialog Button Methods
        public override void DeleteButtonClicked(object obj)
        {
            MainViewModel.GetInstance.DeleteButtonClicked(obj);
            base.DeleteButtonClicked(obj);
        }

        public override void DeleteAcceptButtonClicked(object obj)
        {
            if (SelectedTransaction != null)
            {
                SelectedTransaction.Delete($"{MainViewModel.GetInstance.LoggedInUser.Username} deleted this Transaction on {DateTime.Now} with reason: ({(string)obj})");
                DataAccessLayer.GetInstance.SaveChanges();

                base.DeleteAcceptButtonClicked(obj);

                SelectedTransaction = null;
            }
            OnPropertyChanged(nameof(SelectedTransaction));
            OnPropertyChanged(nameof(AllTransactions));
            MainViewModel.GetInstance.DeleteAcceptButtonClicked(obj);

        }

        public override void DeleteCancelButtonClicked(object obj)
        {
            MainViewModel.GetInstance.DeleteCancelButtonClicked(obj);
            base.DeleteCancelButtonClicked(obj);
        }
        #endregion

        #endregion

        public void SearchContent(string searchString)
        {
            SearchString = searchString;
        }

        /// <summary>
        /// Returns the RealEstate wich contains the RentalObject of a given Transaction
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public RealEstate? GetRealEstateFromTransaction(Transaction? transaction)
        {
            if (transaction == null) return null;
            if (transaction.RentalObject == null) return null;

            if (transaction.RentalObject.Type == RentalObjectType.RealEstateBaseObject)
            {
                return AllRealEstates.FirstOrDefault(x => x.BaseObject != null && x.BaseObject.RentalObjectID == transaction.RentalObject.RentalObjectID);
            }
            else
            {
                return AllRealEstates.FirstOrDefault(x => x.RentalObjects != null && x.RentalObjects.FirstOrDefault(y => y.RentalObjectID == transaction.RentalObject.RentalObjectID) != null);
            }
        }

    }
}
