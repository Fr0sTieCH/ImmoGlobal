using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoGlobalAdmin.MainClasses
{/// <summary>
/// Represents a rental object (Apartement,Garage, Hobbyroom etc)
/// </summary>
    public class RentalObject
    {
        public int RentalObjectID { get; private set; }
        public string RentalObjectName { get; set; }
        public RentalObjectType Type { get; set; }
        public string AddressSupplement { get; set; }
        public double RoomCount { get; set; }
        public double SpaceInQM { get; set; }
        public virtual Person? Owner { get; set; }
        public double EstimatedBaseRent { get; set; }
        public double EstimatedAdditionalCosts { get; set; }
        public virtual BankAccount? Account { get; set; }

        public bool Enabled { get; private set; }
        public string ReasonForDeleting { get; private set; } = "";

        #region CONSTRUCTORS

        public RentalObject()
        {
        }

        /// <summary>
        /// New Rental Object
        /// </summary>
        /// <param name="rentalObjectName">Name of the object</param>
        /// <param name="type">type of the object (Apartement, Garage etc.)</param>
        /// <param name="addressSupplement">Address supplement of the object (Example: "RealEstate Address" + 1.Floor left)</param>
        /// <param name="roomCount">Number of rooms of the object</param>
        /// <param name="spaceInQM">total space in squaremeters of the object</param>
        /// <param name="owner">owner of the object (if null => owner of rental object = owner of realestate)</param>
        /// <param name="estimatedBaseRent">is used to</param>
        /// <param name="realEstate">the realestate to wich this object belongs</param>
        /// <param name="account">Bankaccount on wich all the transactions of this object will get debited or deposited</param>
        public RentalObject(string rentalObjectName, RentalObjectType type, string addressSupplement, double roomCount, double spaceInQM, Person? owner, double estimatedBaseRent, double estimatedAdditionalCosts, RealEstate realEstate, BankAccount account)
        {
            this.RentalObjectName = rentalObjectName;
            this.Type = type;
            this.AddressSupplement = addressSupplement;
            this.RoomCount = roomCount;
            this.SpaceInQM = spaceInQM;
            this.Owner = owner?? realEstate.Owner;
            this.EstimatedBaseRent = estimatedBaseRent;
            this.EstimatedAdditionalCosts = estimatedAdditionalCosts;
            this.Account = account;
            realEstate.AddRentalObject(this);

            this.Enabled = true;
        }

        /// <summary>
        /// Creates a new BaseObject for realestates
        /// </summary>
        /// <param name="realEstate"></param>
        /// <param name="nonRentalRoomCount">Number of rooms wich dont belong to any rental object</param>
        /// <param name="nonRentalSpaceInQM">Space wich is not part of any rental object in squaremeters</param>
        /// <param name="owner">owner of the realEstate</param>
        /// <param name="account">Bankaccount on wich all the transactions of this realestate will get debited or deposited</param>
        public RentalObject(RealEstate realEstate,double nonRentalRoomCount,double nonRentalSpaceInQM,Person owner, BankAccount account)
        {
            this.RentalObjectName = $"{realEstate.RealEstateName}BaseObject";
            this.Type = RentalObjectType.RealEstateBaseObject;
            this.AddressSupplement = "";
            this.RoomCount = nonRentalRoomCount;
            this.SpaceInQM = nonRentalSpaceInQM;
            this.Owner = owner;
            this.EstimatedBaseRent = 0;
            this.EstimatedAdditionalCosts = 0;
            this.Account = account;

            this.Enabled=true;
        }

       
        #endregion


        public void Delete(string reason)
        {
            if (Type == RentalObjectType.RealEstateBaseObject)
            {
                return;
            }

            Enabled = false;
            ReasonForDeleting = reason;
        }
    }
}
