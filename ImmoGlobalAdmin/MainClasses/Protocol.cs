using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoGlobalAdmin.MainClasses
{
    /// <summary>
    /// Represents a protocol
    /// </summary>
    public class Protocol:ImmoGlobalEntity
    {
        public int ProtocolID { get; private set; }
        public virtual RentalObject? RentalObject { get; private set; }
        public virtual RentalContract? RentalContract { get; private set; }
        public ProtocolType Type { get; private set; }
        public virtual ICollection<ProtocolPoint?> Points { get; private set; } = new List<ProtocolPoint?>();
        public DateTime Date { get; private set; }
       
        #region CONSTRUCTORS
        public Protocol() //used by EntityFramework
        {
        }
        #endregion

        #region PUBLIC GETSET
        [NotMapped]
        public RentalObject SetRentalObject
        {
            set => RentalObject = Locked ? RentalObject : value;
        }

        [NotMapped]
        public RentalContract SetRentalContract
        {
            set => RentalContract = Locked ? RentalContract : value;
        }

        [NotMapped]
        public ProtocolType SetType
        {
            set => Type = Locked ? Type : value;
        }

        [NotMapped]
        public List<ProtocolPoint?> SetPoints
        {
            set => Points = Locked ? Points : value;
        }

        [NotMapped]
        public DateTime SetDate
        {
            set => Date = Locked ? Date : value;
        }

        #endregion

       
    }
}
