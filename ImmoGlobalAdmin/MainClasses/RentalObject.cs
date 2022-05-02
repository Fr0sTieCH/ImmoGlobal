using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImmoGlobalAdmin.Helpers;

namespace ImmoGlobalAdmin.MainClasses
{/// <summary>
/// Represents a rental object (Apartement,Garage, Hobbyroom etc)
/// </summary>
    public class RentalObject
    {
        public int RentalObjectID { get; private set; }
        public string RentalObjectName { get; set; } = "";
        public RentalObjectType Type { get; set; }
        public string AddressSupplement { get; set; } = "";
        public double RoomCount { get; set; } = 0;
        public double SpaceInQM { get; set; } = 0;
        public virtual Person? Owner { get; set; }
        public double EstimatedBaseRent { get; set; } = 0;
        public double EstimatedAdditionalCosts { get; set; } = 0;
        public virtual BankAccount? Account { get; set; }
        public virtual ICollection<RentalContract?> RentalContracts { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        public bool HasFridge { get; set; } = false;
        public bool HasDishwasher { get; set; } = false;
        public bool HasStove { get; set; } = false;
        public bool HasWashingmachine { get; set; } = false;
        public bool HasTumbler { get; set; } = false;



        public bool Enabled { get; private set; }
        public string ReasonForDeleting { get; private set; } = "";

        #region CONSTRUCTORS

        public RentalObject()
        {

        }
        public RentalObject(bool IsBaseObject, RealEstate realEstate)
        {
            if (IsBaseObject)
            {
                Enabled = true;
                Type = RentalObjectType.RealEstateBaseObject;
            }
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
            this.Owner = owner ?? realEstate.Owner;
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
        public RentalObject(string realEstateName, double nonRentalRoomCount, double nonRentalSpaceInQM, Person owner, BankAccount account)
        {
            this.RentalObjectName = $"{realEstateName}BaseObject";
            this.Type = RentalObjectType.RealEstateBaseObject;
            this.AddressSupplement = "";
            this.RoomCount = nonRentalRoomCount;
            this.SpaceInQM = nonRentalSpaceInQM;
            this.Owner = owner;
            this.EstimatedBaseRent = 0;
            this.EstimatedAdditionalCosts = 0;
            this.Account = account;

            this.Enabled = true;
        }


        #endregion

        #region PUBLIC GETTERS
        [NotMapped]
        public string TypeIconName
        {
            get
            {
                return EnumTools.GetDescription(this.Type);
            }
            set
            {
                this.Type = EnumTools.GetEnumFromDescriptionAttribute<RentalObjectType>(value);
            }
        } 
 

        [NotMapped]
        public string IGID => RentalObjectID.ToString("2000000000");


        [NotMapped]
        public bool RentalContractActive => ActiveRentalContract != null;
        [NotMapped]
        public bool RentStatusOK
        {
            get
            {
                if (RentsDue.Count>1)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
        }
        [NotMapped]
        public bool RentStatusNotOK => !RentStatusOK;

        [NotMapped]
        public RentalContract? ActiveRentalContract
        {
            get
            {
                if (RentalContracts == null)
                {
                    return null;
                }
                else
                {
                    foreach(RentalContract rc in RentalContracts)
                    {
                        rc.CheckState();
                    }
                    return RentalContracts.FirstOrDefault(x => x.State == ContractState.Active);
                }
            }

        }

        [NotMapped]
        public List<Transaction?> PayedRents => Transactions.Where(x => x.Type == TransactionType.Rent).ToList();

        [NotMapped]
        public List<DateTime> RentsDue
        {
            get
            {
                if (RentalContractActive)
                {
                    List<DateTime> tmp = new List<DateTime>();

                    //loop throught every month since the active contract started
                    for (DateTime d = ActiveRentalContract.StartDate.Date; d.Date < DateTime.Now.AddMonths(1).Date; d = d.AddMonths(1))
                    {
                        
                        tmp.Add(DateTime.Parse($"{ActiveRentalContract.RentDueDay},{d.Month},{d.Year}"));
                    }

                    if (PayedRents.Count(x => x.AssociatedPerson == ActiveRentalContract.Tenant) == 0)
                    {
                        return tmp;
                    }
                    else
                    {


                        foreach (Transaction t in PayedRents.Where(x => x.AssociatedPerson == ActiveRentalContract.Tenant))
                        {

                            if (t.DateTimeOfTransaction > ActiveRentalContract.StartDate.AddMonths(-1).Date)//in case one tenant was tenant of this object before => only account transactions wich are less than 1 month before the startdate of the contract
                            {
                                tmp.RemoveAt(0);
                                //foreach payment remove one duedate
                            }
                        }

                        return tmp;
                    }
                }
                else
                {
                    return new List<DateTime>();
                }
            }
        }

        #endregion



        public void Delete(string reason)
        {
            if (Type == RentalObjectType.RealEstateBaseObject) //Every realestate has to have a baseobject, therefore deleting of a realestateobject is not allowed
            {
                return;
            }

            Enabled = false;
            ReasonForDeleting = reason;
        }

        public void DeleteBaseObject(string reason)
        {

            Enabled = false;
            ReasonForDeleting = reason;
        }
    }
}
