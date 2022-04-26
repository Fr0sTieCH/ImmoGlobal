﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoGlobalAdmin.MainClasses
{/// <summary>
/// Represents a physical bankaccount
/// </summary>
    public class BankAccount
    {
        public int BankAccountID { get; private set; }
        public string AccountName { get; set; }
        public string Iban { get; set; }
        public virtual ICollection<Transaction?> Transactions { get; private set; }

        public bool Enabled { get; private set; }
        public string ReasonForDeleting { get; private set; }= "";

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
            this.Transactions = new List<Transaction>();
            this.Enabled = true;
        }

        public BankAccount()
        {
        }
        #endregion

      
        /// <summary>
        /// Calculates the current balance based on all transactions associatet with this bankaccount
        /// </summary>
        /// <returns></returns>
        /// 
        public double CurrentBalance()
        {
            return this.Transactions.Where(x => x.Enabled).Sum(x =>  x.Value);
        }

        public void AddTransaction(Transaction transaction)
        {
            Transactions.Add(transaction);
        }

        public void Delete(string reason)
        {
            Enabled = false;
            ReasonForDeleting = reason;
        }


        
    }
}