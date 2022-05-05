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
        private RealEstate? realEstateToSelectObjects;




        #region Binding Properties
        public override List<Transaction> AllTransactions
        {
            get
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
                    List<RentalObject> list = realEstateToSelectObjects.RentalObjects.ToList();
                    list.Add(realEstateToSelectObjects.BaseObject);
                    return list;
                }
            }
        }


        #endregion

        public string SearchString
        {
            get { return searchString; }
            set
            {
                searchString = value;
                OnPropertyChanged(nameof(AllBankAccounts));
            }
        }

        public void SearchContent(string searchString)
        {
            SearchString = searchString;
        }


        #region Buttons

        protected override void CancelEditButtonClicked(object obj)
        {

            SelectedTransaction = null;

            base.CancelEditButtonClicked(obj);

            OnPropertyChanged(nameof(SelectedTransaction));
            OnPropertyChanged(nameof(AllTransactions));
        }


        protected override void SaveEditButtonClicked(object obj)
        {
            SelectedTransaction.Lock();
            DataAccessLayer.GetInstance.StoreNewTransaction(SelectedTransaction);
            SelectedTransaction = null;
            realEstateToSelectObjects = null;

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

        #endregion

        #region Delete Dialog Button Overrides
        public override void DeleteButtonClicked(object obj)
        {
            MainViewModel.GetInstance.DeleteButtonClicked(obj);
            base.DeleteButtonClicked(obj);
        }

        public override void DeleteAcceptButtonClicked(object obj)
        {
            SelectedTransaction.Delete($"{MainViewModel.GetInstance.LoggedInUser.Username} deleted this Transaction on {DateTime.Now} with reason: ({(string)obj})");
            DataAccessLayer.GetInstance.SaveChanges();

            base.DeleteAcceptButtonClicked(obj);

            SelectedTransaction = null;
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

    }
}
