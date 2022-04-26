using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ImmoGlobalAdmin.MainClasses;


namespace ImmoGlobalAdmin.Model
{
    internal class ImmoGlobalContext:DbContext
    {
        public DbSet<BankAccount> bankAccounts { get; set; }
        public DbSet<Invoice> invoices { get; set; }
        public DbSet<Person> persons { get; set; }
        public DbSet<Protocol>protocols { get; set; }
        public DbSet<ProtocolPoint> protocolPoints { get; set; }
        public DbSet<RealEstate> realEstates { get; set; }
        public DbSet<RentalContract> rentalContracts { get; set; }
        public DbSet<RentalObject> rentalObjects { get; set; }
        public DbSet<Transaction> transactions { get; set; }
        public DbSet<User> users { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Test");
            optionsBuilder.UseLazyLoadingProxies();
        }

    }
}
