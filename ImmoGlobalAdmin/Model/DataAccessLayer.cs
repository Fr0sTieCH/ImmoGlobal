using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImmoGlobalAdmin.MainClasses;

namespace ImmoGlobalAdmin.Model
{
    /// <summary>
    /// Accessing (Store,Edit,Delete) data from the specified database using entity framework core
    /// </summary>
    internal class DataAccessLayer
    {
        private ImmoGlobalContext _db;

        #region Singleton
        private static DataAccessLayer? instance = null;
        private static readonly object padlock = new();

        protected DataAccessLayer()
        {
            using (ImmoGlobalContext context = new ImmoGlobalContext())
            {
                context.Database.EnsureCreated();
            }
            //create new context object
            //make sure we always use the same context-object to be able to lazyload the objects
            //this approach will be changed in the real application, as it uses a lot of memory
            _db = new ImmoGlobalContext();

        }

        /// <summary>
        /// returns instance of class DataAccessLayer
        /// </summary>
        public static DataAccessLayer GetInstance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new DataAccessLayer();
                    }
                    return instance;
                }
            }
        }
        #endregion

        public void SaveChanges()
        {
            _db.SaveChanges();
        }

        public void DisposeContext()
        {
            //db.Dispose();
            //db = new ImmoGlobalContext();
        }
        public void RestoreValuesFromDB(object entity)
        {
            _db.Entry(entity).Reload();
        }

        #region Store
        public void StoreNewUser(User newUser)
        {
            _db.users.Add(newUser);
            SaveChanges();
        }

        public void StoreNewTransaction(Transaction newTransaction)
        {
            _db.transactions.Add(newTransaction);
            SaveChanges();
        }

        public void StoreNewRentalObject(RentalObject newRentalObject)
        {
            _db.rentalObjects.Add(newRentalObject);
            SaveChanges();
        }

        public void StoreNewRentalContract(RentalContract newRentalContract)
        {
            _db.rentalContracts.Add(newRentalContract);
            SaveChanges();
        }

        public void StoreNewRealEstate(RealEstate newRealEstate)
        {
            _db.realEstates.Add(newRealEstate);
            SaveChanges();
        }

        public void StoreNewProtocol(Protocol newProtocol)
        {
            _db.protocols.Add(newProtocol);
            SaveChanges();
        }

        public void StoreNewPerson(Person newPerson)
        {
            _db.persons.Add(newPerson);
            SaveChanges();
        }

        public void StoreNewInvoice(Invoice newInvoice)
        {
            _db.invoices.Add(newInvoice);
            SaveChanges();
        }

        public void StoreNewBankAccount(BankAccount newBankAccount)
        {
            _db.bankAccounts.Add(newBankAccount);
            SaveChanges();
        }


        #endregion

        #region Retrieve

        public User? GetUserByName(string username)
        {
            return _db.users.Where(x => x.Username == username && x.Enabled).FirstOrDefault();
        }

        public List<RealEstate> GetRealEstatesUnfiltered()
        {
            return _db.realEstates.Where(x => x.Enabled).ToList();
        }

        public List<Person> GetPersonsUnfiltered()
        {
            return _db.persons.Where(x => x.Enabled).ToList();
        }

        public List<Person> GetEmployeesUnfiltered()
        {
            return _db.persons.Where(x => x.Enabled && x.Type == PersonType.Employee).ToList();
        }

        public List<Person> GetPrivatePersonsUnfiltered()
        {
            return _db.persons.Where(x => x.Enabled && x.Type == PersonType.PrivatePerson).ToList();
        }

        public List<Person> GetCompaniesUnfiltered()
        {
            return _db.persons.Where(x => x.Enabled && x.Type == PersonType.Company).ToList();
        }

        public List<BankAccount> GetBankAccountsUnfiltered()
        {
            return _db.bankAccounts.Where(x => x.Enabled).ToList();
        }

        public List<Transaction> GetTransactionsUnfiltered()
        {
            return _db.transactions.Where(x => x.Enabled).ToList();
        }

        public List<Transaction> GetTransactionsByRentalObject(RentalObject rentalObject)
        {
            if (rentalObject == null) return new List<Transaction>();
            return _db.transactions.Where(x => x.Enabled && x.RentalObject != null && x.RentalObject.RentalObjectID == rentalObject.RentalObjectID).ToList();
        }

        public List<RentalContract> GetContractsUnfiltered()
        {
            return _db.rentalContracts.Where(x => x.Enabled).ToList();
        }
        #endregion


    }
}
