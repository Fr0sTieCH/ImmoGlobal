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

        private List<Person> aviablePersons = new List<Person>();
        private List<BankAccount> aviableBankAccounts = new List<BankAccount>();
        private List<RealEstate> aviableRealEstates = new List<RealEstate>();

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
            StoreTestJuristicPersonsToDB();
            StoreTestBankAccountsToDB();
            StoreTestRealEstatesToDB();
            StoreTestUsersToDB();
            StoreTestRentalContractsToDB();

        }



        #region Test Persons
        private void StoreTestPersonsToDB() //Create Persons and stor them in the db (if they dont exist already)
        {
            aviablePersons = DataAccessLayer.GetInstance.GetPersonsUnfiltered();

            foreach (Person p in TestPersonsFromCSV("TestPersons"))
            {
                if (!PersonExists(p))
                {
                    DataAccessLayer.GetInstance.StoreNewPerson(p);
                }
            }
        }

        private void StoreTestJuristicPersonsToDB()
        {
            aviablePersons = DataAccessLayer.GetInstance.GetPersonsUnfiltered();

            foreach (Person p in TestJuristicPersonsFromCSV("TestPersons(JuristicEntities)"))
            {
                if (!JuristicPersonExists(p))
                {
                    DataAccessLayer.GetInstance.StoreNewPerson(p);
                }
            }
        }

        private List<Person> TestPersonsFromCSV(string filename)
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + @$"\TestData\{filename}.csv";
            string[] lines = File.ReadAllLines(filePath);

            return lines.Select(line =>
            {
                string[] data = line.Split(';');
                string name = data[0];
                string prename = data[1];
                string address = data[2];
                string phone = data[3];
                string fax = data[4];
                string email = data[5];
                DateTime birthdate = DateTime.Parse(data[6]);
                string note = data[7];
                int type = int.Parse(data[8]);

                Person newPerson = new Person(name, prename, address, phone, fax, email, birthdate, note);
                newPerson.Type = (PersonType)type;
                return newPerson;
            }).ToList();
        }

        private List<Person> TestJuristicPersonsFromCSV(string filename)
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + @$"\TestData\{filename}.csv";
            string[] lines = File.ReadAllLines(filePath);

            return lines.Select(line =>
            {
                string[] data = line.Split(';');
                string name = data[0];
                string adress = data[1];
                string phone = data[2];
                string fax = data[3];
                string email = data[4];
                string vatNumber = data[5];
                string note = data[6];
                int type = int.Parse(data[7]);

                Person newPerson = new Person(name, adress, phone, fax, email, vatNumber, note);
                newPerson.Type = (PersonType)type;
                return newPerson;
            }).ToList();
        }
        private bool JuristicPersonExists(Person personToCheck)
        {
            return aviablePersons.Exists(x => x.Name == personToCheck.Name && x.VatNuber == personToCheck.VatNuber && x.Adress == personToCheck.Adress);
        }

        private bool PersonExists(Person personToCheck)
        {
            return aviablePersons.Exists(x => x.Name == personToCheck.Name && x.Prename == personToCheck.Prename && x.Adress == personToCheck.Adress && x.Birthdate == personToCheck.Birthdate);
        }
        #endregion

        #region Test BankAccounts
        private void StoreTestBankAccountsToDB() //Create Persons and stor them in the db (if they dont exist already)
        {
            aviableBankAccounts = DataAccessLayer.GetInstance.GetBankAccountsUnfiltered();

            foreach (BankAccount ba in TestBankAccountsFromCSV("TestBankAccounts"))
            {
                if (!BankAccountExists(ba))
                {
                    DataAccessLayer.GetInstance.StoreNewBankAccount(ba);
                }
            }
        }

        private List<BankAccount> TestBankAccountsFromCSV(string filename)
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + @$"\TestData\{filename}.csv";
            string[] lines = File.ReadAllLines(filePath);

            return lines.Select(line =>
            {
                string[] data = line.Split(';');
                string name = data[0];
                string iban = data[1];


                return new BankAccount(name, iban);
            }).ToList();
        }

        private bool BankAccountExists(BankAccount bankAccountToCheck)
        {
            return aviableBankAccounts.Exists(x => x.AccountName == bankAccountToCheck.AccountName && x.Iban == bankAccountToCheck.Iban);
        }
        #endregion

        #region Test RealEstates

        private void StoreTestRealEstatesToDB()
        {
            aviableRealEstates = DataAccessLayer.GetInstance.GetRealEstatesUnfiltered();

            foreach (RealEstate re in TestRealEstatesFromCSV("TestRealEstates"))
            {
                if (!RealEstateExists(re))
                {
                    DataAccessLayer.GetInstance.StoreNewRealEstate(re);
                }
            }
        }

        private List<RealEstate> TestRealEstatesFromCSV(string filename)
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + @$"\TestData\{filename}.csv";
            string[] lines = File.ReadAllLines(filePath);

            return lines.Select(line =>
            {
                string[] data = line.Split(';');
                string name = data[0];
                string address = data[1];
                string rooms = data[2];
                string qm = data[3];
                Person owner = DataAccessLayer.GetInstance.GetPersonsUnfiltered().Where(x => x.Name == "CreditSuisse").First();
                Person janitor = new Person($"{name}", "Janitor", address, "079 235 56 32", "", $"{name}janitor@gmail.com", DateTime.Parse("01.01.1980"), "Test janitor, would be a real person");
                RealEstate tmpRE = new RealEstate(name, address, owner, janitor, double.Parse(rooms), double.Parse(qm), DataAccessLayer.GetInstance.GetBankAccountsUnfiltered()[0]);

                for (int i = 0; i < 4; i++)//create appartements
                {
                    RentalObject newObject = new RentalObject($"{i + 1}.Appartment of {tmpRE.RealEstateName}", RentalObjectType.Apartement, $"Ap.{i + 1}", 3, 72.5, tmpRE.Owner, 900, 200, tmpRE, tmpRE.Account);
                    newObject.ResponsibleEmployee = DataAccessLayer.GetInstance.GetPersonsUnfiltered().FirstOrDefault(x=> x.Name=="Test" && x.Prename=="Anna");
                }
                for (int i = 0; i < 4; i++)//create Garages
                {
                    RentalObject newObject = new RentalObject($"{i + 1}.Garage of {tmpRE.RealEstateName}", RentalObjectType.Garage, "", 1, 12, tmpRE.Owner, 60, 10, tmpRE, tmpRE.Account);
                    newObject.ResponsibleEmployee = DataAccessLayer.GetInstance.GetPersonsUnfiltered().FirstOrDefault(x => x.Name == "Test" && x.Prename == "Anna");

                }
                for (int i = 0; i < 2; i++)//create hobbyrooms
                {
                    RentalObject newObject = new RentalObject($"{i + 1}.Hobbyroom in {tmpRE.RealEstateName}", RentalObjectType.Hobby, $"HR.{i + 1}", 1, 12, tmpRE.Owner, 100, 20, tmpRE, tmpRE.Account);
                    newObject.ResponsibleEmployee = DataAccessLayer.GetInstance.GetPersonsUnfiltered().FirstOrDefault(x => x.Name == "Test" && x.Prename == "Anna");

                }

                return tmpRE;

            }).ToList();
        }

        private bool RealEstateExists(RealEstate realEstateToCheck)
        {
            return aviableRealEstates.Exists(x => x.RealEstateName == realEstateToCheck.RealEstateName);
        }
        #endregion

        #region Test Users

        private void StoreTestUsersToDB()
        {
            if (aviablePersons.Count < 3)
            {
                return;
            }
            else
            {
                foreach(Person p in DataAccessLayer.GetInstance.GetEmployeesUnfiltered())
                {

                    string username = $"{p.Name}{p.Prename}";

                    if (DataAccessLayer.GetInstance.GetUserByName(username) == null)
                    {
                        DataAccessLayer.GetInstance.StoreNewUser(new User(username, p, "Passwort1234", Permissions.Admin));
                    }
                }
                
                  
                
            }

        }

        #endregion

        #region test RentalContracts

        private void StoreTestRentalContractsToDB()
        {
            Random random = new Random();
            List<Person> persons = DataAccessLayer.GetInstance.GetPersonsUnfiltered();
            Debug.WriteLine("-----Generating Rental Contracts-----");

            foreach (RealEstate re in DataAccessLayer.GetInstance.GetRealEstatesUnfiltered())
            {
                Debug.WriteLine($"Generating for: {re.RealEstateID} ");
                foreach (RentalObject ro in re.RentalObjects)
                {
                    Debug.WriteLine($"Generating for object: {ro.RentalObjectName}");
                    Debug.WriteLine("Generating Rental Contracts");
                    Person randomTenant = persons[random.Next(0, persons.Count - 1)];

                    RentalContract newRentalContract = new RentalContract(randomTenant,persons.First(x=>x.Name=="Test"&&x.Prename=="Anna"),ro,900,200,5,DateTime.Parse("01.01.2022"));
                    newRentalContract.ValidateContract();

                    //ro.RentalContracts.Add(newRentalContract);
                    DataAccessLayer.GetInstance.StoreNewRentalContract(newRentalContract);
                    DataAccessLayer.GetInstance.SaveChanges();

                }
            }

            Debug.WriteLine("----- Finished Generating Rental Contracts-----");
        }


        #endregion
    }
}

