using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoGlobalAdmin.MainClasses
{
    public class Invoice:ImmoGlobalEntity
    {
        public int InvoiceID { get; private set; }
        public string Title { get; private set; }
        public double Sum { get; private set; }
        public virtual RentalObject? RentalObject { get; private set; }
        public virtual RentalContract? RentalContract { get; private set; }
        public virtual ICollection<Transaction?> Transactions { get; private set; }
        public DateTime Date { get; private set; }
        public virtual Person AssociatedPerson { get; private set; }


        #region CONSTRUCTORS
        public Invoice()
        {
        }

        public Invoice(string title, RentalObject rentalObject, RentalContract rentalContract, List<Transaction> transactions, DateTime date, Person associatedPerson)
        {
            this.Title = title;
            this.RentalObject = rentalObject;
            this.RentalContract = rentalContract;
            this.Transactions = transactions;
            this.Date = date;
            this.AssociatedPerson = associatedPerson;
            this.Enabled = true;
            this.Locked = false;

            CalculateSum();
        }
        #endregion

        #region PUBLIC SETTERS
        [NotMapped]
        public string SetTitle
        {
            set => Title = Locked ? Title : value;
        }

        [NotMapped]
        public DateTime SetDate
        {
            set => Date = Locked ? Date : value;
        }

        [NotMapped]
        public RentalContract SetRentalContract
        {
            set => RentalContract = Locked ? RentalContract : value;
        }

        [NotMapped]
        public List<Transaction> SetTransactions
        {
            set => Transactions = Locked ? Transactions : value;
        }

        [NotMapped]
        public Person SetAssociatedPerson
        {
            set => AssociatedPerson = Locked ? AssociatedPerson : value;
        }
        #endregion


        public void CalculateSum()
        {
            if (Locked) { return; }
            Sum = Transactions.Sum(x=>x.Value);
        }

     
    }
}
