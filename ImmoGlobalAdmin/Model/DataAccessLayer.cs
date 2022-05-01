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
        private ImmoGlobalContext db;

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
            db = new ImmoGlobalContext();

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
            db.SaveChanges();
        }
        public void RestoreValuesFromDB(object entity)
        {
            db.Entry(entity).Reload();
        }

        #region Store
        public void StoreNewUser(User newUser)
        {
            db.users.Add(newUser);
            SaveChanges();
        }

        public void StoreNewTransaction(Transaction newTransaction)
        {
            db.transactions.Add(newTransaction);
            SaveChanges();
        }

        public void StoreNewRentalObject(RentalObject newRentalObject)
        {
            db.rentalObjects.Add(newRentalObject);
            SaveChanges();
        }

        public void StoreNewRentalContract(RentalContract newRentalContract)
        {
            db.rentalContracts.Add(newRentalContract);
            SaveChanges();
        }

        public void StoreNewRealEstate(RealEstate newRealEstate)
        {
            db.realEstates.Add(newRealEstate);
            SaveChanges();
        }

        public void StoreNewProtocol(Protocol newProtocol)
        {
            db.protocols.Add(newProtocol);
            SaveChanges();
        }

        public void StoreNewPerson(Person newPerson)
        {
            db.persons.Add(newPerson);
            SaveChanges();
        }

        public void StoreNewInvoice(Invoice newInvoice)
        {
            db.invoices.Add(newInvoice);
            SaveChanges();
        }

        public void StoreNewBankAccount(BankAccount newBankAccount)
        {
            db.bankAccounts.Add(newBankAccount);
            SaveChanges();
        }


        #endregion

        #region Retrieve

        public User? GetUserByName(string username)
        {
            return db.users.Where(x => x.Username == username && x.Enabled).FirstOrDefault();
        }

        public List<RealEstate> GetRealEstatesUnfiltered()
        {
            return db.realEstates.Where(x => x.Enabled).ToList();
        }

        public List<Person> GetPersonsUnfiltered()
        {
            return db.persons.Where(x => x.Enabled).ToList();
        }


        public List<BankAccount> GetBankAccountsUnfiltered()
        {
            return db.bankAccounts.Where(x => x.Enabled).ToList();
        }
    
        #endregion


    }
}
