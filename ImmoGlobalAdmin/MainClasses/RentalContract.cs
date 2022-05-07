﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoGlobalAdmin.MainClasses
{/// <summary>
/// Represents a rental contract
/// </summary>
    public class RentalContract:ImmoGlobalEntity
    {
        public int RentalContractID { get; private set; }
        public virtual Person? Tenant { get; private set; }
        public virtual Person? ResponsibleEmployee { get; private set; }
        public virtual RentalObject? RentalObject { get; private set; }
        public double BaseRent { get; private set; }
        public double AdditionalCosts { get; private set; }
        public int RentDueDay { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }
        public ContractState State { get; private set; }

     

        #region CONSTRUCTORS
        public RentalContract()
        {
            
        }

        public RentalContract(RentalObject rentalObject,DateTime startDate)
        {

            this.ResponsibleEmployee = rentalObject.ResponsibleEmployee;
            this.RentalObject = rentalObject;
            this.BaseRent = rentalObject.EstimatedBaseRent;
            this.AdditionalCosts = rentalObject.EstimatedAdditionalCosts;
            this.StartDate = startDate;
            this.RentDueDay = 1;
            this.State = ContractState.ValidationPending;
            this.Enabled = true;
            this.Locked = false;

        }

        /// <summary>
        /// New rental Contract
        /// </summary>
        /// <param name="tenant">The tenant of the rental object</param>
        /// <param name="responsibleEmployee">The employee who's responsible for the rental object</param>
        /// <param name="rentalObject">The rental object</param>
        /// <param name="baseRent"> base rent per month wich without any additional costs</param>
        /// <param name="additionalCosts">additional costs per month</param>
        /// <param name="startDate">start of the rental contract</param>
        public RentalContract(Person tenant, Person responsibleEmployee, RentalObject rentalObject, double baseRent, double additionalCosts,int rentDueDay, DateTime startDate)
        {
            this.Tenant = tenant;
            this.ResponsibleEmployee = responsibleEmployee;
            this.RentalObject = rentalObject;
            this.BaseRent = baseRent;
            this.AdditionalCosts = additionalCosts;
            this.StartDate = startDate;
            this.RentDueDay = rentDueDay;
            this.State = ContractState.ValidationPending;
            this.Enabled = true;
            this.Locked = false;
        }

        /// <summary>
        /// New rental contract based on a reference contract
        /// </summary>
        /// <param name="referenceContract"></param>
        /// <param name="tenant"></param>
        /// <param name="responsibleEmployee"></param>
        /// <param name="startDate"></param>
        public RentalContract(RentalContract referenceContract, Person tenant, Person responsibleEmployee, DateTime startDate)
        {
            this.Tenant = tenant;
            this.ResponsibleEmployee = responsibleEmployee;
            this.RentalObject = referenceContract.RentalObject;
            this.BaseRent = referenceContract.BaseRent;
            this.AdditionalCosts = referenceContract.AdditionalCosts;
            this.StartDate = startDate;
            this.State = ContractState.ValidationPending;
            this.Enabled = true;
            this.Locked = false;
        }

        #endregion

        #region PUBLIC SETTERS
        [NotMapped]
        public double ChangeBaseRent
        {
            get => BaseRent;
            set => BaseRent = Locked ? BaseRent : value;
        }

        [NotMapped]
        public double ChangeAdditionalCosts
        {
            get => AdditionalCosts;
            set => AdditionalCosts = Locked ? AdditionalCosts : value;
        }

        [NotMapped]
        public DateTime ChangeStartDate
        {
            get => StartDate;
            set => StartDate = Locked ? StartDate : value;
        }

        [NotMapped]
        public DateTime? ChangeEndDate 
        { 
            get => EndDate;
            set => EndDate = State == ContractState.RunningOut? value : EndDate; //only allowed to change while the contract is running out
        }

        [NotMapped]
        public Person? ChangeTenant
        {
            get => Tenant;
            set => Tenant = Locked ? Tenant : value;
        }

        [NotMapped]
        public Person? ChangeResponsibleEmployee
        {
            get => ResponsibleEmployee;
            set => ResponsibleEmployee = Locked ? ResponsibleEmployee : value;
        }

        [NotMapped]
        public int ChangeRentDueDay
        {
            get => RentDueDay;
            set => RentDueDay = Locked ? RentDueDay : value;
        }

        #endregion

        #region PUBLIC GETTERS
        [NotMapped]
        public string IGID => RentalContractID.ToString("RC00000000");
        [NotMapped]
        public double RentTotal => BaseRent + AdditionalCosts;
        [NotMapped]
        public bool IsCanceled => State > ContractState.Active;
        [NotMapped]
        public bool IsActive => State == ContractState.Active;
        [NotMapped]
        public bool IsNotLocked => State == ContractState.ValidationPending;
        #endregion

        /// <summary>
        /// Validates the rental contract
        /// </summary>
        public void ValidateContract()
        {
            if (State == ContractState.ValidationPending)
            {
                State = ContractState.Validated;
                Locked = true;
            }
            CheckState();
        }
        /// <summary>
        /// Changes the state of the contract to "RunningOut" 
        /// </summary>
        /// <param name="endDate">The date on wich the contract will get terminated</param>
        public void TerminateContract(DateTime endDate)
        {
            if (State == ContractState.Active)
            {
                State = ContractState.RunningOut;
                Locked = true;
            }

            this.EndDate = endDate;
            CheckState();
        }

        /// <summary>
        /// Checks the state of the contract and changes it if needed (based on current date and the parameters startDate and/or endDate)
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public void CheckState()
        {
            DateTime today = DateTime.Now.Date;


            switch (State)
            {
                case ContractState.Active:
                    if (StartDate > today)
                    {
                        State = ContractState.Validated;
                        Locked = true;
                    }
                    break;
                case ContractState.RunningOut:
                    if (EndDate == null)
                    {
                        State = ContractState.Active;
                        Locked = true;
                    }
                    else if (EndDate <= today)
                    {
                        State = ContractState.Terminated;
                        Locked = true;
                    }
                    break;
                case ContractState.Validated:
                    if (StartDate < today)
                    {
                        State = ContractState.Active;
                        Locked = true;
                    }
                    break;
                case ContractState.Terminated:
                    if (EndDate == null)
                    {
                        throw new ArgumentException("Parameter cannot be null if contract is already terminated!", nameof(EndDate));
                    }
                    else if (EndDate > today)
                    {
                        State = ContractState.RunningOut;
                        Locked = true;
                    }
                    break;
                default:
                    {
                        Locked = false;
                    }
                    break;
            }


        }

    }
}
