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
        Rent,
        AdditionalCosts,
        RentIncAdditionalCosts,
        Electricity,
        Water,
        Heating,
        Telecomunication,
        Repair
    }

    public enum Permissions
    {
        Admin,
        RentalObjectManager,
        FinanceManager
    }
}
