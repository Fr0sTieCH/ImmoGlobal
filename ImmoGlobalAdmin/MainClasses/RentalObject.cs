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
    public class RentalObject : ImmoGlobalEntity
    {
        public int RentalObjectID { get; private set; }
        public string RentalObjectName { get; set; } = "";
        public RentalObjectType Type { get; set; } = RentalObjectType.Apartement;
        public string AddressSupplement { get; set; } = "";
        public double RoomCount { get; set; } = 0;
        public double SpaceInQM { get; set; } = 0;
        public virtual Person? Owner { get; set; }
        public virtual Person? ResponsibleEmployee { get; set; }
        public double EstimatedBaseRent { get; set; } = 0;
        public double EstimatedAdditionalCosts { get; set; } = 0;
        public virtual BankAccount? Account { get; set; }
        public virtual ICollection<RentalContract?> RentalContracts { get; set; } = new List<RentalContract?>();
        public virtual ICollection<Transaction?> Transactions { get; set; } = new List<Transaction?>();
        public bool HasFridge { get; set; } = false;
        public bool HasDishwasher { get; set; } = false;
        public bool HasStove { get; set; } = false;
        public bool HasWashingmachine { get; set; } = false;
        public bool HasTumbler { get; set; } = false;


        #region CONSTRUCTORS

        public RentalObject()//used by EntityFramework
        {

        }

        public RentalObject(RentalObject refBaseOject)
        {
            Owner = refBaseOject.Owner;
            ResponsibleEmployee = refBaseOject.ResponsibleEmployee;
            Account = refBaseOject.Account;
            Enabled = true;
        }

        public RentalObject(bool IsBaseObject, RealEstate realEstate)
        {
            if (IsBaseObject)
            {

                Type = RentalObjectType.RealEstateBaseObject;
            }

            Enabled = true;
        }


        #endregion

        #region PUBLIC GETSET
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
        public string IGID
        {
            get
            {
                if (this.Type == RentalObjectType.RealEstateBaseObject)
                {
                    return RentalObjectID.ToString("RE 00000000");
                }
                else
                {
                    return RentalObjectID.ToString("OB 00000000");
                }
            }
        }

        [NotMapped]
        public double EstimatedRentTotal => EstimatedBaseRent + EstimatedAdditionalCosts;

        [NotMapped]
        public bool RentalContractActive => ActiveRentalContract != null;

        /// <summary>
        /// Returns true if Tenant is not in default of payment
        /// </summary>
        [NotMapped]
        public bool RentStatusOK => RentBalance >= 0;

        /// <summary>
        /// Returns true if Tenant is in default of payment
        /// </summary>
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
                    foreach (RentalContract? rc in RentalContracts)
                    {
                        if (rc != null)
                        {
                            rc.CheckState();
                        }
                    }
                    return RentalContracts.FirstOrDefault(x => x != null && x.State == ContractState.Active || x != null && x.State == ContractState.RunningOut);
                }
            }

        }

        [NotMapped]
        public List<Transaction?> PayedRents => Transactions.Where(x => x != null && x.Type == TransactionType.Rent).ToList();

        [NotMapped]
        public Transaction? LastPayedRent => Transactions.OrderByDescending(x => x.DateTimeOfTransaction).Where(x => x.Type == TransactionType.Rent).FirstOrDefault();

        /// <summary>
        /// Returns the current Balance => <0 = Tenant is in default of payment >0 Tenant payed to much
        /// </summary>
        [NotMapped]
        public double RentBalance
        {
            get
            {
                if (RentalContractActive)
                {
                    double owedRent = 0;

                    //loop throught every month since the active contract started
                    for (DateTime d = ActiveRentalContract.StartDate.Date; d.Date < DateTime.Now.Date; d = d.AddMonths(1))
                    {
                        owedRent += ActiveRentalContract.RentTotal;
                    }
                    //List of all the Transactions of the current Tenant (only transactions wich were payed after the start of the contract - 1 month)
                    List<Transaction?> transactionsOfActiveTenant = PayedRents.Where(x => x.AssociatedPerson == ActiveRentalContract.Tenant
                                                                               && x.DateTimeOfTransaction > ActiveRentalContract.StartDate.AddMonths(-1).Date)
                                                                               .ToList();
                    //total sum of payments
                    double sumPayments = transactionsOfActiveTenant.Sum(x => x.Value);

                    return sumPayments - owedRent;
                }
                else
                {
                    return 0;
                }
            }
        }

        #endregion


        /// <summary>
        /// Creates Adds and returns a new Rental contract
        /// StartDate gets set to the enddate of the active rentalcontract or to today if there's none
        /// </summary>
        /// <returns></returns>
        public RentalContract CreateNewRentalContract()
        {
            DateTime? suggestedStartDate = null;
            if (ActiveRentalContract != null)
            {
                suggestedStartDate = ActiveRentalContract.EndDate;
            }

            RentalContract newRC = new RentalContract(this, suggestedStartDate ?? DateTime.Now);
            RentalContracts.Add(newRC);

            return newRC;
        }

        /// <summary>
        /// Removes a given RentalContract (Only use this if the rentalcontract is not already stored to the DB)
        /// </summary>
        /// <param name="contractToRemove"></param>
        public void RemoveRentalContract(RentalContract contractToRemove)
        {
            RentalContracts.Remove(contractToRemove);
        }

        protected override void DeleteLogic(string reason)
        {
            if (Type == RentalObjectType.RealEstateBaseObject) //Every realestate has to have a baseobject, therefore deleting of a realestatebaseobject is not allowed
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
