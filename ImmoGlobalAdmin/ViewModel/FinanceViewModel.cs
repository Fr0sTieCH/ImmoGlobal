using ImmoGlobalAdmin.MainClasses;
using ImmoGlobalAdmin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoGlobalAdmin.ViewModel
{
    internal class FinanceViewModel:BaseViewModel,IHasSearchableContent
    {
        private string _searchString = "";
        private BankAccount? _selectedBankAccount;
        #region Singleton
        private static FinanceViewModel? instance = null;
        private static readonly object padlock = new();

        public FinanceViewModel()
        {
        }

        /// <summary>
        /// returns instance of class FinanceViewModel
        /// </summary>
        public static FinanceViewModel GetInstance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new FinanceViewModel();
                    }
                    return instance;
                }
            }
        }

        #endregion


        #region Binding Properties

        public override List<BankAccount> AllBankAccounts
        {
            get
            {
                if (_searchString == "" || _searchString == null)
                {
                    return DataAccessLayer.GetInstance.GetBankAccountsUnfiltered();
                }
                else
                {
                    //make changes to the search logic here...
                    return DataAccessLayer.GetInstance.GetBankAccountsUnfiltered().Where(x => x.AccountName.ToLower().StartsWith(SearchString.ToLower())).ToList();
                }

            }
        }

        public string SearchString
        {
            get { return _searchString; }
            set
            {
                _searchString = value;
                OnPropertyChanged(nameof(AllBankAccounts));
            }
        }

        public BankAccount? SelectedBankAccount
        {
            get
            {
                if (_selectedBankAccount == null)
                {
                    _selectedBankAccount = AllBankAccounts.FirstOrDefault();
                }
                return _selectedBankAccount;
            }
            set
            {
                if (_editMode || DeleteDialogOpen)
                {
                    return;
                }
                _selectedBankAccount = value;
                OnPropertyChanged(nameof(SelectedBankAccount));
            }
        }
        #endregion

        public void SearchContent(string searchString)
        {
            SearchString = searchString;
        }

        #region Buttons
        protected override void EditButtonClicked(object obj)
        {
            OnPropertyChanged(nameof(SelectedBankAccount));
            OnPropertyChanged(nameof(AllBankAccounts));
            base.EditButtonClicked(obj);
        }

        protected override void CancelEditButtonClicked(object obj)
        {
            if (_creationMode)
            {
                SelectedBankAccount = null;
            }
            else
            {
                DataAccessLayer.GetInstance.RestoreValuesFromDB(SelectedBankAccount);
            }

            base.CancelEditButtonClicked(obj);

            OnPropertyChanged(nameof(SelectedBankAccount));
            OnPropertyChanged(nameof(AllBankAccounts));
        }


        protected override void SaveEditButtonClicked(object obj)
        {
            if (_creationMode)
            {
                DataAccessLayer.GetInstance.StoreNewBankAccount(SelectedBankAccount);
                SelectedBankAccount= null;
            }
            else
            {
                DataAccessLayer.GetInstance.SaveChanges();
            }

            base.SaveEditButtonClicked(obj);

            OnPropertyChanged(nameof(SelectedBankAccount));
            OnPropertyChanged(nameof(AllBankAccounts));

        }

        protected override void CreateButtonClicked(object obj)
        {
            SelectedBankAccount = new BankAccount(true);
            OnPropertyChanged(nameof(SelectedBankAccount));
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
            SelectedBankAccount.Delete($"{MainViewModel.GetInstance.LoggedInUser.Username} deleted this BankAccount on {DateTime.Now} with reason: ({(string)obj})");
            DataAccessLayer.GetInstance.SaveChanges();

            base.DeleteAcceptButtonClicked(obj);

            SelectedBankAccount = null;
            OnPropertyChanged(nameof(SelectedBankAccount));
            OnPropertyChanged(nameof(AllBankAccounts));
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
