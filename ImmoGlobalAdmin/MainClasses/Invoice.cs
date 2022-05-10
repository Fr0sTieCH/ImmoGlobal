using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoGlobalAdmin.MainClasses
{
    /// <summary>
    /// Represents an invoice
    /// </summary>
    public class Invoice : ImmoGlobalEntity
    {
        public int InvoiceID { get; private set; }
        public string Title { get; private set; } = "";
        public double Sum { get; private set; }
        public virtual RentalObject? RentalObject { get; private set; }
        public virtual RentalContract? RentalContract { get; private set; }
        public virtual ICollection<Transaction?> Transactions { get; private set; } = new List<Transaction?>();
        public DateTime Date { get; private set; }
        public virtual Person AssociatedPerson { get; private set; }


        #region CONSTRUCTORS
        public Invoice()//used by EntityFramework
        {
        }
        #endregion

        #region PUBLIC GETSET
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
        public List<Transaction?> SetTransactions
        {
            set => Transactions = Locked ? Transactions : value;
        }

        [NotMapped]
        public Person SetAssociatedPerson
        {
            set => AssociatedPerson = Locked ? AssociatedPerson : value;
        }
        #endregion

        /// <summary>
        /// calculets the sum of all referenced transactions
        /// </summary>
        public void CalculateSum()
        {
            if (Locked) { return; }
            Sum = Transactions.Sum(x => x != null ? x.Value : 0);
        }


    }
}
