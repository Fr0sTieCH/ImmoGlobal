using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoGlobalAdmin.MainClasses
{
    public enum RentalObjectType
    {
        RealEstateBaseObject,
        Apartement,
        Garage,
        Hobby,
        Office,
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
        ManageUsers,
        EditRealEstate,
        EditTransactions,
        ManageRentalObjects,
        CreateInvoices
    }
}
