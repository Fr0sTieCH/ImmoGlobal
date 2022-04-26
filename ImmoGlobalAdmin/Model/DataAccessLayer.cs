using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImmoGlobalAdmin.MainClasses;

namespace ImmoGlobalAdmin.Model
{
    internal class DataAccessLayer
    {
       
            private ImmoGlobalContext db;

            public DataAccessLayer(ImmoGlobalContext _db) => this.db = _db;

            #region Store/Delete
            public void CreateNewUser(User _newUser)
            {
                db.users.Add(_newUser);
                db.SaveChanges();
            }


            public void UpdateDB()
            {
                db.SaveChanges();
            }

        
            #endregion

            #region Retrieve
            public User GetUserByName(string _username)
            {
                return db.users.Where(x => x.Username == _username).FirstOrDefault();
            }

          

            #endregion

        
    }
}
