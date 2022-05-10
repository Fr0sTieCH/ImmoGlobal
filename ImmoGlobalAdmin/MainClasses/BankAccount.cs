using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoGlobalAdmin.MainClasses
{/// <summary>
/// Represents a physical bankaccount
/// </summary>
    public class BankAccount:ImmoGlobalEntity
    {
        public int BankAccountID { get; private set; }
        public string AccountName { get; set; } = "";
        public string Iban { get; set; } = "";
        public virtual ICollection<Transaction?> Transactions { get; private set; } = new List<Transaction?>();
       
        #region CONSTRUCTORS
        /// <summary>
        /// New Bankaccount
        /// </summary>
        /// <param name="accountName"> name of the account</param>
        /// <param name="iban"> IBAN of the account</param>
        /// <exception cref="ArgumentNullException"></exception>
        public BankAccount(string accountName, string iban)
        {
            this.AccountName = accountName;
            this.Iban = iban;
            this.Enabled = true;
        }

        public BankAccount()//used by EntityFramework
        {
        }

        public BankAccount(bool enabled)
        {
            this.Enabled = enabled;
        }
        #endregion

        #region PUBLIC GETSET

        [NotMapped]
        public string IGID => BankAccountID.ToString("BA 00000000");

        /// <summary>
        /// Calculates the current balance based on all transactions associatet with this bankaccount
        /// </summary>
        /// <returns></returns>
        /// 
        [NotMapped]
        public double CurrentBalance
        {
            get
            {
                return this.Transactions.Where(x => x!=null && x.Enabled).Sum(x => x!=null? x.Value:0);
            }      
        }
        #endregion


        public void AddTransaction(Transaction transaction)
        {
            Transactions.Add(transaction);
        }

    }
}
