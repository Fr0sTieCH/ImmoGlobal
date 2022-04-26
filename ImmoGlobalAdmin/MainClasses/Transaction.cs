using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoGlobalAdmin.MainClasses
{
    public class Transaction
    {
        public int TransactionID { get; private set; }
        public virtual RentalObject? RentalObject { get; private set; }
        public DateTime DateTimeOfTransaction { get; private set; }
        public double Value { get; private set; }
        public TransactionType Type { get; private set; }
        public virtual Person? AssociatedPerson { get; private set; }
        public string Note { get; set; }

        public bool Enabled { get; private set; }
        public string ReasonForDeleting { get; private set; } = "";

        public Transaction()
        {
        }

        #region CONSTRUCTORS

        /// <summary>
        /// Creates a new Transaction using the objects default bankaccount
        /// </summary>
        /// <param name="rentalObject"></param>
        /// <param name="dateTimeOfTransaction"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <param name="associatedPerson"></param>
        /// <param name="note"></param>
        public Transaction(RentalObject rentalObject, DateTime dateTimeOfTransaction, double value, TransactionType type, Person associatedPerson, string note)
        {
            this.RentalObject = rentalObject;
            this.DateTimeOfTransaction = dateTimeOfTransaction;
            this.Value = value;
            this.Type = type;
            this.AssociatedPerson = associatedPerson;
            this.Note = note;
            rentalObject.Account.AddTransaction(this);

            this.Enabled = true;
        }

        /// <summary>
        /// Creates a new Transaction using a specified bankaccount
        /// </summary>
        /// <param name="rentalObject"></param>
        /// <param name="targetAccount"></param>
        /// <param name="dateTimeOfTransaction"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <param name="associatedPerson"></param>
        /// <param name="note"></param>
        public Transaction(RentalObject rentalObject, BankAccount targetAccount, DateTime dateTimeOfTransaction, double value, TransactionType type, Person associatedPerson, string note)
        {
            this.RentalObject = rentalObject;
            this.DateTimeOfTransaction = dateTimeOfTransaction;
            this.Value = value;
            this.Type = type;
            this.AssociatedPerson = associatedPerson;
            this.Note = note;
            targetAccount.AddTransaction(this);

            this.Enabled = true;
        }

        #endregion

        public void Delete(string reason)
        {
            Enabled = false;
            ReasonForDeleting = reason;
        }
    }
}
