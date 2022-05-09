using ImmoGlobalAdmin.Commands;
using ImmoGlobalAdmin.Model;
using ImmoGlobalAdmin.MainClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.IO;
using System.Diagnostics;
using System.Security;

namespace ImmoGlobalAdmin.ViewModel
{
    internal class EFTestingViewModel : BaseViewModel
    {

        #region Singleton
        private static EFTestingViewModel? instance = null;
        private static readonly object padlock = new();

        public EFTestingViewModel()
        {

        }

        /// <summary>
        /// returns instance of class HomeViewModel
        /// </summary>
        public static EFTestingViewModel GetInstance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new EFTestingViewModel();
                    }
                    return instance;
                }
            }
        }
        #endregion

        private List<Person> _aviablePersons = new List<Person>();
        private List<BankAccount> _aviableBankAccounts = new List<BankAccount>();
        private List<RealEstate> _aviableRealEstates = new List<RealEstate>();

        public ICommand TestButtonCommand
        {
            get
            {
                return new RelayCommand<object>(TestButtonClicked);
            }
        }

        private void TestButtonClicked(object obj)
        {
            DataAccessLayer.GetInstance.SaveChanges();

            StoreTestPersonsToDB();
            StoreTestUsersToDB();
            StoreBankAccountsToDB();
            StoreTestRealEstatesIncludingRentalObjects();
            StoreTransactions();
            //StoreTestJuristicPersonsToDB();
            //StoreTestBankAccountsToDB();
            //StoreTestRealEstatesToDB();
            //StoreTestRentalContractsToDB();

            //System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            //Application.Current.Shutdown();
        }

        //Gets all lines exept the first from csv
        private string[] GetDataLinesFromCSV(string filename)
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + @$"\TestData\{filename}";
            return File.ReadAllLines(filePath).Skip(1).ToArray();
        }


     
        private void StoreTestPersonsToDB() //Create Persons and stor them in the db (if they dont exist already)
        {
            foreach (string s in GetDataLinesFromCSV("TestPersonen.csv"))
            {
                string[] data = s.Split(';');

                Person newPerson = new Person(true);
                newPerson.Type = (PersonType)int.Parse(data[0]);
                newPerson.Sex = (Sex)int.Parse(data[1]);
                newPerson.Name = data[2];
                newPerson.Prename = data[3];
                newPerson.Adress = data[4];
                newPerson.Birthdate = data[5] == "" ? null : DateTime.Parse(data[5]);
                newPerson.Phone = data[6];
                newPerson.EMail = data[7];
                newPerson.VatNuber = data[8];
                newPerson.Note = data[9];

                DataAccessLayer.GetInstance.StoreNewPerson(newPerson);
            }
        }

        private void StoreTestUsersToDB()
        {

            foreach (Person p in DataAccessLayer.GetInstance.GetEmployeesUnfiltered())
            {
                string username = $"{p.Name}{p.Prename}";
                if (DataAccessLayer.GetInstance.GetUserByName(username) == null)
                {

                    DataAccessLayer.GetInstance.StoreNewUser(new User(username, p, "Passwort1234", Permissions.Admin));
                }
            }

        }

        private void StoreBankAccountsToDB()
        {
            foreach (string s in GetDataLinesFromCSV("TestBankAccounts.csv"))
            {
                string[] data = s.Split(';');

                BankAccount newAccount = new BankAccount(true);

                newAccount.AccountName = data[0];
                newAccount.Iban = data[1];

                DataAccessLayer.GetInstance.StoreNewBankAccount(newAccount);
            }


        }

        private void StoreTestRealEstatesIncludingRentalObjects()
        {
            foreach (string s in GetDataLinesFromCSV("TestLiegenschaften.csv"))
            {
                string[] data = s.Split(';');

                RealEstate newRealEstate = new RealEstate(true);

                newRealEstate.RealEstateName = data[0];
                newRealEstate.BaseObject.RentalObjectName = data[0];
                newRealEstate.Address = data[1];
                newRealEstate.BaseObject.RoomCount = double.Parse(data[2]);
                newRealEstate.BaseObject.SpaceInQM = double.Parse(data[3]);
                newRealEstate.BuildingInsurance = data[4];
                newRealEstate.LiabilityInsurance = data[5];
                newRealEstate.PersonalInsurance = data[6];
                newRealEstate.BaseObject.Owner = DataAccessLayer.GetInstance.GetPersonsUnfiltered().FirstOrDefault(x => x.Name.StartsWith("ImmoGlobal"));
                newRealEstate.Janitor = DataAccessLayer.GetInstance.GetPersonsUnfiltered().FirstOrDefault(x => x.Name == "Test");
                newRealEstate.BaseObject.Account = DataAccessLayer.GetInstance.GetBankAccountsUnfiltered()[2];


                { DataAccessLayer.GetInstance.StoreNewRealEstate(newRealEstate); }




            }

            int appartementCount = 0;
            Person[] tenantsOfRealEstate = DataAccessLayer.GetInstance.GetPersonsUnfiltered().Where(x => x.Name == "Mueller" || x.Name == "Koch" || x.Name == "Suter").ToArray();

            foreach (string s in GetDataLinesFromCSV("TestMietObjekteLiegenschaft1.csv"))
            {
                string[] data = s.Split(';');

                RentalObject newRentalObject = DataAccessLayer.GetInstance.GetRealEstatesUnfiltered()[0].AddRentalObject();

                newRentalObject.Type = (RentalObjectType)int.Parse(data[0]);
                newRentalObject.RentalObjectName = data[1];
                newRentalObject.AddressSupplement = data[2];
                newRentalObject.RoomCount = double.Parse(data[3]);
                newRentalObject.SpaceInQM = double.Parse(data[4]);
                newRentalObject.EstimatedBaseRent = double.Parse(data[5]);
                newRentalObject.EstimatedAdditionalCosts = double.Parse(data[6]);
                newRentalObject.HasFridge = data[7] == "" ? false : true;
                newRentalObject.HasDishwasher = data[8] == "" ? false : true;
                newRentalObject.HasStove = data[9] == "" ? false : true;
                newRentalObject.HasWashingmachine = data[10] == "" ? false : true;
                newRentalObject.HasTumbler = data[11] == "" ? false : true;
                newRentalObject.Owner = DataAccessLayer.GetInstance.GetRealEstatesUnfiltered()[0].Owner;
                newRentalObject.Account = DataAccessLayer.GetInstance.GetRealEstatesUnfiltered()[0].Account;
                newRentalObject.ResponsibleEmployee = DataAccessLayer.GetInstance.GetPersonsUnfiltered().FirstOrDefault(x => x.Name == "Beispiel");

                DataAccessLayer.GetInstance.StoreNewRentalObject(newRentalObject);
                DataAccessLayer.GetInstance.SaveChanges();

                if (newRentalObject.Type == RentalObjectType.Apartement)
                {
                    RentalContract newContract = newRentalObject.CreateNewRentalContract();
                    newContract.ChangeStartDate = DateTime.Parse("01.01.2022");
                    newContract.ChangeResponsibleEmployee = newRentalObject.ResponsibleEmployee;
                    newContract.ChangeTenant = tenantsOfRealEstate[appartementCount];
                    newContract.ChangeRentDueDay = 5;
                    newContract.ValidateContract();

                    appartementCount++;
                }

            }

            appartementCount = 0;
            tenantsOfRealEstate = DataAccessLayer.GetInstance.GetPersonsUnfiltered().Where(x => x.Name == "Obama" || x.Name == "Jordi").ToArray();


            foreach (string s in GetDataLinesFromCSV("TestMietObjekteLiegenschaft2.csv"))
            {
                string[] data = s.Split(';');

                RentalObject newRentalObject = DataAccessLayer.GetInstance.GetRealEstatesUnfiltered()[1].AddRentalObject();

                newRentalObject.Type = (RentalObjectType)int.Parse(data[0]);
                newRentalObject.RentalObjectName = data[1];
                newRentalObject.AddressSupplement = data[2];
                newRentalObject.RoomCount = double.Parse(data[3]);
                newRentalObject.SpaceInQM = double.Parse(data[4]);
                newRentalObject.EstimatedBaseRent = double.Parse(data[5]);
                newRentalObject.EstimatedAdditionalCosts = double.Parse(data[6]);
                newRentalObject.HasFridge = data[7] == "" ? false : true;
                newRentalObject.HasDishwasher = data[8] == "" ? false : true;
                newRentalObject.HasStove = data[9] == "" ? false : true;
                newRentalObject.HasWashingmachine = data[10] == "" ? false : true;
                newRentalObject.HasTumbler = data[11] == "" ? false : true;
                newRentalObject.Owner = DataAccessLayer.GetInstance.GetRealEstatesUnfiltered()[1].Owner;
                newRentalObject.Account = DataAccessLayer.GetInstance.GetRealEstatesUnfiltered()[1].Account;
                newRentalObject.ResponsibleEmployee = DataAccessLayer.GetInstance.GetPersonsUnfiltered().FirstOrDefault(x => x.Name == "Beispiel");


                DataAccessLayer.GetInstance.StoreNewRentalObject(newRentalObject);

                if (newRentalObject.Type == RentalObjectType.Apartement)
                {
                    RentalContract newContract = newRentalObject.CreateNewRentalContract();
                    newContract.ChangeStartDate = DateTime.Parse("01.01.2022");
                    newContract.ChangeResponsibleEmployee = newRentalObject.ResponsibleEmployee;
                    newContract.ChangeTenant = tenantsOfRealEstate[appartementCount];
                    newContract.ChangeRentDueDay = 5;
                    newContract.ValidateContract();

                    DataAccessLayer.GetInstance.StoreNewRentalContract(newContract);

                    appartementCount++;
                }
            }

            appartementCount = 0;
            tenantsOfRealEstate = DataAccessLayer.GetInstance.GetPersonsUnfiltered().Where(x => x.Name == "Verstappen" || x.Name == "Leclerc" || x.Name == "Steiner" || x.Name == "Horner").ToArray();


            foreach (string s in GetDataLinesFromCSV("TestMietObjekteLiegenschaft3.csv"))
            {
                string[] data = s.Split(';');

                RentalObject newRentalObject = DataAccessLayer.GetInstance.GetRealEstatesUnfiltered()[2].AddRentalObject();

                newRentalObject.Type = (RentalObjectType)int.Parse(data[0]);
                newRentalObject.RentalObjectName = data[1];
                newRentalObject.AddressSupplement = data[2];
                newRentalObject.RoomCount = double.Parse(data[3]);
                newRentalObject.SpaceInQM = double.Parse(data[4]);
                newRentalObject.EstimatedBaseRent = double.Parse(data[5]);
                newRentalObject.EstimatedAdditionalCosts = double.Parse(data[6]);
                newRentalObject.HasFridge = data[7] == "" ? false : true;
                newRentalObject.HasDishwasher = data[8] == "" ? false : true;
                newRentalObject.HasStove = data[9] == "" ? false : true;
                newRentalObject.HasWashingmachine = data[10] == "" ? false : true;
                newRentalObject.HasTumbler = data[11] == "" ? false : true;
                newRentalObject.Owner = DataAccessLayer.GetInstance.GetRealEstatesUnfiltered()[2].Owner;
                newRentalObject.Account = DataAccessLayer.GetInstance.GetRealEstatesUnfiltered()[2].Account;
                newRentalObject.ResponsibleEmployee = DataAccessLayer.GetInstance.GetPersonsUnfiltered().FirstOrDefault(x => x.Name == "Beispiel");


                DataAccessLayer.GetInstance.StoreNewRentalObject(newRentalObject);

                if (newRentalObject.Type == RentalObjectType.Apartement)
                {
                    RentalContract newContract = newRentalObject.CreateNewRentalContract();
                    newContract.ChangeStartDate = DateTime.Parse("01.01.2022");
                    newContract.ChangeResponsibleEmployee = newRentalObject.ResponsibleEmployee;
                    newContract.ChangeTenant = tenantsOfRealEstate[appartementCount];
                    newContract.ChangeRentDueDay = 5;
                    newContract.ValidateContract();

                    DataAccessLayer.GetInstance.StoreNewRentalContract(newContract);

                    appartementCount++;
                }
            }

        }

        private void StoreTransactions()
        {
            Random rnd = new Random();

            foreach(RentalContract rc in DataAccessLayer.GetInstance.GetContractsUnfiltered())
            {

                for(int i = 0; i < 4; i++)
                {
                    Transaction newTransaction = new Transaction(true);
                    newTransaction.SetValue = rc.RentTotal;
                    newTransaction.SetRentalObject = rc.RentalObject;
                    newTransaction.SetBankAccount = rc.RentalObject.Account;
                    newTransaction.SetType = TransactionType.Rent;
                    newTransaction.SetDateTimeOfTransaction = DateTime.Parse("01.01.2022").AddMonths(i);
                    newTransaction.SetAssociatedPerson = rc.Tenant;
                    newTransaction.Lock();
                    DataAccessLayer.GetInstance.StoreNewTransaction(newTransaction);

                }

                int rndValue = rnd.Next(0,100);

                if (rndValue > 90)
                {
                    Transaction newTransaction = new Transaction(true);
                    newTransaction.SetValue = rc.RentTotal + 500;
                    newTransaction.SetRentalObject = rc.RentalObject;
                    newTransaction.SetBankAccount = rc.RentalObject.Account;
                    newTransaction.SetType = TransactionType.Rent;
                    newTransaction.SetDateTimeOfTransaction = DateTime.Parse("01.01.2022").AddMonths(5);
                    newTransaction.SetAssociatedPerson = rc.Tenant;
                    newTransaction.Lock();
                    DataAccessLayer.GetInstance.StoreNewTransaction(newTransaction);
                }
                else if(rndValue>25)
                {
                    Transaction newTransaction = new Transaction(true);
                    newTransaction.SetValue = rc.RentTotal;
                    newTransaction.SetRentalObject = rc.RentalObject;
                    newTransaction.SetBankAccount = rc.RentalObject.Account;
                    newTransaction.SetType = TransactionType.Rent;
                    newTransaction.SetDateTimeOfTransaction = DateTime.Parse("01.01.2022").AddMonths(5);
                    newTransaction.SetAssociatedPerson = rc.Tenant;
                    newTransaction.Lock();
                    DataAccessLayer.GetInstance.StoreNewTransaction(newTransaction);
                }

            }
        }
       
     
      
    }
}

