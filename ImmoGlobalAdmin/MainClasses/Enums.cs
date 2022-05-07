using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoGlobalAdmin.MainClasses
{
    public enum RentalObjectType
    {
        //Descriptions are used to display the corresponding icons
        RealEstateBaseObject,
        [Description("Home")]
        Apartement,
        [Description("Garage")]
        Garage,
        [Description("ChildToy")]
        Hobby,
        [Description("OfficeChair")]
        Office,
        [Description("CarPark")]
        ParkingSpot
    }

    public enum Sex
    {
        Male,
        Female,
        Other
    }

    public enum PointCondition
    {
        NotOk,
        Ok,
        New
    }

    public enum ProtocolType
    {
        Handover,
        Takeover
    }

    public enum ContractState
    {
        ValidationPending,
        Validated,
        Active,
        RunningOut,
        Terminated
    }

    public enum TransactionType
    {
        Deposit,
        Rent,
        Electricity,
        Water,
        Heating,
        Telecommunication,
        Repair,
        OtherT
    }

    public enum Permissions
    {
        Admin,
        RentalObjectManager,
        FinanceManager
    }

    public enum PersonType
    {
        PrivatePerson,
        Company,
        Employee
    }
}
