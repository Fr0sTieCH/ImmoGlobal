using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoGlobalAdmin.MainClasses
{
    public class Protocol:ImmoGlobalEntity
    {
        public int ProtocolID { get; private set; }
        public virtual RentalObject? RentalObject { get; private set; }
        public virtual RentalContract? RentalContract { get; private set; }
        public ProtocolType Type { get; private set; }
        public virtual ICollection<ProtocolPoint?> Points { get; private set; }
        public DateTime Date { get; private set; }
        #region CONSTRUCTORS

        /// <summary>
        /// Creates a new Protocol
        /// </summary>
        /// <param name="rentalObject"></param>
        /// <param name="rentalContract"></param>
        /// <param name="type"></param>
        /// <param name="points"></param>
        /// <param name="date"></param>
        public Protocol(RentalObject rentalObject, RentalContract rentalContract, ProtocolType type, List<ProtocolPoint> points, DateTime date)
        {
            this.RentalObject = rentalObject;
            this.RentalContract = rentalContract;
            this.Type = type;
            this.Points = points;
            this.Date = date;
            this.Enabled = true;
            this.Locked = false;
        }
        
        /// <summary>
        /// Creates a new protocol by using a reference protocol
        /// </summary>
        /// <param name="type"></param>
        /// <param name="referenceProtocol"></param>
        /// <param name="date"></param>
        public Protocol(ProtocolType type, Protocol referenceProtocol, DateTime date)
        {
            this.RentalObject = referenceProtocol.RentalObject;
            this.RentalContract = referenceProtocol.RentalContract;
            this.Type = type;
            this.Points = referenceProtocol.Points;
            this.Date = date;
            this.Enabled = true;
            this.Locked = false;
        }

        public Protocol()
        {
        }
        #endregion

        #region PUBLIC SETTERS
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
        public List<ProtocolPoint> SetPoints
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
