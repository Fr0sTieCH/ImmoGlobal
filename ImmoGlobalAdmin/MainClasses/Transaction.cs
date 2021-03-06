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


        #region CONSTRUCTORS
        public Transaction()//used by EntityFramework
        {
        }

        public Transaction(bool enabled)
        {
            this.Enabled = enabled;
        }
        #endregion



        #region PUBLIC GETSET
        [NotMapped]
        public string IGID => TransactionID.ToString("TR00000000");

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
