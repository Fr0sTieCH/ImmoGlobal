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
        public virtual RentalObject? BaseObject { get; set; }

        private bool Enabled { get; set; }
        private string ReasonForDeleting { get; set; } = "";



        #region CONSTRUCTORS
        public RealEstate()
        {
        }

        public RealEstate(string realEstateName, string address, Person owner, Person janitor, List<RentalObject> rentalObjects,double nonRentalRoomCount, double nonRentalSpaceinQM,BankAccount account)
        {
            this.RealEstateName = realEstateName;
            this.Address = address;
            this.Janitor = janitor;
            this.RentalObjects = rentalObjects;
            //generate a baseObject for the realEstate
            this.BaseObject = new RentalObject(this, nonRentalRoomCount, nonRentalSpaceinQM, owner, account);

            this.Enabled = true;
        }
        #endregion


        #region PUBLIC GETTERS
        [NotMapped]
        public Person Owner => BaseObject.Owner;

        [NotMapped]
        public BankAccount Account => BaseObject.Account;

        [NotMapped]
        public double RoomCount => BaseObject.RoomCount;

        [NotMapped]
        public double SpaceInQM=> BaseObject.SpaceInQM;
        #endregion

        public void AddRentalObject(RentalObject rentalObject)
        {
            RentalObjects.Add(rentalObject);
        }     

        public void Delete(string reason)
        {
            foreach(RentalObject ro in RentalObjects)
            {
                ro.Delete($"Deleted because the realestate got deleted with the reason:{reason}"); 
            }
            Enabled = false;
            ReasonForDeleting = reason;
        }
    }
}
