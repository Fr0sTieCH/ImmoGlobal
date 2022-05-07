using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoGlobalAdmin.MainClasses
{
    public class Transaction:ImmoGlobalEntity
    {
        public int TransactionID { get; private set; }
        public virtual RentalObject? RentalObject { get; private set; }
        public virtual BankAccount? BankAccount { get; private set; }
        public DateTime? DateTimeOfTransaction { get; private set; }
        public double Value { get; private set; }
        public TransactionType Type { get; private set; } = TransactionType.Deposit;
        public virtual Person? AssociatedPerson { get; private set; }
        public string Note { get; set; } = "";

        public Transaction()
        {
        }
        public Transaction(bool enabled)
        {
            this.Enabled = enabled;
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

        [NotMapped]
        public string IGID => TransactionID.ToString("TR00000000");

        #region PUBLIC SETTERS
        [NotMapped]
        public RentalObject? SetRentalObject 
        {
            get => RentalObject;
            set => RentalObject = Locked ? RentalObject : value;
        }
        [NotMapped]
        public BankAccount? SetBankAccount
        {
            get => BankAccount;
            set => BankAccount = Locked ? BankAccount : value;
        }
        [NotMapped]
        public DateTime? SetDateTimeOfTransaction
        {
            get => DateTimeOfTransaction;
            set => DateTimeOfTransaction = Locked ? DateTimeOfTransaction : value;
        }
        [NotMapped]
        public double SetValue
        {
            get => Value;
            set => Value = Locked? Value:value;
        }
        [NotMapped]
        public TransactionType SetType
        {
            get => Type;
            set => Type = Locked? Type: value;
        }
        [NotMapped]
        public string? SetTypeString
        {
            get => Enum.GetName(Type);
            set => Type = Locked ? Type : Enum.Parse<TransactionType>(value);
        }
        [NotMapped]
        public Person? SetAssociatedPerson
        {
            get => AssociatedPerson;
            set => AssociatedPerson = Locked ? AssociatedPerson : value;
        }
        [NotMapped]
        public string SetNote
        {
            get => Note;
            set => Note = Locked ? Note : value;
        }

        #endregion

    }
}
