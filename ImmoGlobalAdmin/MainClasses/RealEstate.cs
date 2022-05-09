﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoGlobalAdmin.MainClasses
{
    public class RealEstate:ImmoGlobalEntity
    {
        public int RealEstateID { get; private set; }
        public string RealEstateName { get; set; } = "";
        public string Address { get; set; } = "";
        public string BuildingInsurance { get; set; } = "";
        public string LiabilityInsurance { get; set; } = "";
        public string PersonalInsurance { get; set; } = "";
        public virtual Person? Janitor { get; set; }
        public virtual ICollection<RentalObject?> RentalObjects { get; set; }
        public virtual RentalObject BaseObject { get; private set; }


        #region CONSTRUCTORS
        public RealEstate()
        {
        }

        public RealEstate(bool enabled)
        {
            
            this.BaseObject = new RentalObject(true,this);
            this.RentalObjects = new List<RentalObject>();
            this.Enabled = enabled;
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
        public string IGID => BaseObject.IGID;

        [NotMapped]
        public Person Owner => BaseObject.Owner;

        [NotMapped]
        public BankAccount Account => BaseObject.Account;

        [NotMapped]
        public double RoomCount => BaseObject.RoomCount;

        [NotMapped]
        public double SpaceInQM=> BaseObject.SpaceInQM;

        [NotMapped]
        public ICollection<RentalObject?> SortedRentalObjects => RentalObjects.OrderBy(x => x.Type).ToList();

        [NotMapped]
        public double TotalObjectCount => RentalObjects.Count;

        [NotMapped]
        public double TotalRentalRooms => RentalObjects.Sum(x => x.RoomCount);

        [NotMapped]
        public double TotalRentalSpace => RentalObjects.Sum(x=> x.SpaceInQM);
        #endregion

        /// <summary>
        /// Creates and returns a new empty RentalObject and adds it to this RealEstate
        /// </summary>
        public RentalObject AddRentalObject()
        {
            RentalObject newRO = new RentalObject(this.BaseObject);
            RentalObjects.Add(newRO);
            return newRO;
        }
        
        /// <summary>
        /// Removes a given objet from RentalObjects (Use this only if the RentalObject is not yet saved to the DB)
        /// </summary>
        /// <param name="objectToRemove"></param>
        public void RemoveRentalObject(RentalObject objectToRemove)
        {
            RentalObjects.Remove(objectToRemove);
        }

        protected override void DeleteLogic(string reason)
        {
            BaseObject.DeleteBaseObject($"Deleted because the realestate got deleted with the reason:{reason}");

            if (RentalObjects != null && RentalObjects.Count > 0)
            {
                foreach (RentalObject ro in RentalObjects)
                {
                    ro.Delete($"Deleted because the realestate got deleted with the reason:{reason}");
                }
            }
            base.DeleteLogic(reason);
        }
    }
}
