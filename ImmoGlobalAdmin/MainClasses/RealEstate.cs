using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoGlobalAdmin.MainClasses
{
    public class RealEstate
    {
        public int RealEstateID { get; private set; }
        public string RealEstateName { get; set; }
        public string Address { get; set; }
        public virtual Person? Janitor { get; set; }
        public virtual ICollection<RentalObject?> RentalObjects { get; set; }
        public virtual RentalObject? BaseObject { get; private set; }

        public bool Enabled { get; private set; }
        public string ReasonForDeleting { get; private set; } = "";



        #region CONSTRUCTORS
        public RealEstate()
        {
        }

        public RealEstate(bool enabled)
        {
            Enabled = enabled;
        }

        public RealEstate(string realEstateName, string address, Person owner, Person janitor, double nonRentalRoomCount, double nonRentalSpaceinQM,BankAccount account)
        {
            this.RealEstateName = realEstateName;
            this.Address = address;
            this.Janitor = janitor;
            RentalObjects = new List<RentalObject>();
            //generate a baseObject for the realEstate
            this.BaseObject = new RentalObject(realEstateName, nonRentalRoomCount, nonRentalSpaceinQM, owner, account);

            this.Enabled = true;
        }
        #endregion


        #region PUBLIC GETTERS
        [NotMapped]
        public string IGID => BaseObject.RentalObjectID.ToString("1000000000");

        [NotMapped]
        public Person Owner => BaseObject.Owner;

        [NotMapped]
        public BankAccount Account => BaseObject.Account;

        [NotMapped]
        public double RoomCount => BaseObject.RoomCount;

        [NotMapped]
        public double SpaceInQM=> BaseObject.SpaceInQM;

        [NotMapped]
        public double TotalObjectCount => RentalObjects.Count;

        [NotMapped]
        public double TotalRentalRooms => RentalObjects.Sum(x => x.RoomCount);

        [NotMapped]
        public double TotalRentalSpace => RentalObjects.Sum(x=> x.SpaceInQM);
        #endregion

        public void AddRentalObject(RentalObject rentalObject)
        {
            RentalObjects.Add(rentalObject);
        }     

        public void Delete(string reason)
        {
            BaseObject.DeleteBaseObject($"Deleted because the realestate got deleted with the reason:{reason}");
            foreach(RentalObject ro in RentalObjects)
            {
                ro.Delete($"Deleted because the realestate got deleted with the reason:{reason}"); 
            }
            Enabled = false;
            ReasonForDeleting = reason;
        }
    }
}
