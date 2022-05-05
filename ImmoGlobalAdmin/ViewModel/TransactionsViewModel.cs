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
    internal class TransactionsViewModel : BaseViewModel, IHasSearchableContent
    {

        private string searchString = "";
        private Transaction? selectedTransaction;
        private RealEstate? realEstateToSelectObjects;//used for choosing a rentalObject on creation
        private RentalObject? rentalObjectToFilterTransactions = null;

        #region constructors
        public TransactionsViewModel() { }
        public TransactionsViewModel(RentalObject rentalObjectToFilterTransactions)
        {
            this.rentalObjectToFilterTransactions = rentalObjectToFilterTransactions;
        }
        #endregion

        #region Binding Properties
        public override List<Transaction> AllTransactions
        {
            get
            {
                if (rentalObjectToFilterTransactions == null)//if not null, get only Transactions of the specified rental object
                {
                    if (searchString == "" || searchString == null)
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
                        return DataAccessLayer.GetInstance.GetTransactionsByRentalObject(rentalObjectToFilterTransactions); 
                }

            }
        }


        public Transaction? SelectedTransaction
        {
            get
            {
                if (selectedTransaction == null)
                {
                    selectedTransaction = AllTransactions.FirstOrDefault();
                }
                return selectedTransaction;
            }
            set
            {
                if (editMode || DeleteDialogOpen)
                {
                    return;
                }
                selectedTransaction = value;
                RealEstateToSelectObjects = GetRealEstateFromTransaction(value);
                OnPropertyChanged(nameof(RealEstateToSelectObjects));
                OnPropertyChanged(nameof(SelectedTransaction));
            }
        }

        public RealEstate? RealEstateToSelectObjects
        {
            get
            {
                if (realEstateToSelectObjects == null)
                {
                    realEstateToSelectObjects = AllRealEstates.FirstOrDefault();
                }
                return realEstateToSelectObjects;
            }
            set
            {
                realEstateToSelectObjects = value;
                OnPropertyChanged(nameof(RealEstateToSelectObjects));
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

                    list.Insert(0,realEstateToSelectObjects.BaseObject);
                    return list;
                }
            }
        }

        public string SearchString
        {
            get { return searchString; }
            set
            {
                searchString = value;
                OnPropertyChanged(nameof(AllTransactions));
            }
        }
        #endregion

        #region Button Methods
        protected override void CancelEditButtonClicked(object obj)
        {
            SelectedTransaction = null;
            base.CancelEditButtonClicked(obj);
            OnPropertyChanged(nameof(SelectedTransaction));
            OnPropertyChanged(nameof(AllTransactions));
        }

        protected override void SaveEditButtonClicked(object obj)
        {
            if (SelectedTransaction != null)
            {
                SelectedTransaction.Lock();
                DataAccessLayer.GetInstance.StoreNewTransaction(SelectedTransaction);
                SelectedTransaction = null;
                realEstateToSelectObjects = null;
            }

            base.SaveEditButtonClicked(obj);
            OnPropertyChanged(nameof(SelectedTransaction));
            OnPropertyChanged(nameof(AllTransactions));
        }

        protected override void CreateButtonClicked(object obj)
        {
            SelectedTransaction = new Transaction(true);
            OnPropertyChanged(nameof(SelectedTransaction));
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
