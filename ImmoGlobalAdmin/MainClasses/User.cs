using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoGlobalAdmin.MainClasses
{/// <summary>
/// represents an user
/// WARNING: NOT SECURE, FOR PROTOTYPE ONLY!!
/// </summary>
    public class User:ImmoGlobalEntity
    {
        public int UserID { get; private set; }
        public string Username { get; private set; }
        public virtual Person? Person { get;  set; }
        public string Password { get; private set; }
        public Permissions PermissionLevel { get; set; }


        public User()
        {
        }

        #region Constructors
        /// <summary>
        /// Creates a new User
        /// </summary>
        /// <param name="username"></param>
        /// <param name="person"></param>
        /// <param name="password"></param>
        /// <param name="permissions"></param>
        public User(string username, Person person, string password, Permissions permissionLevel)
        {
            this.Enabled = true;
            this.Username = username;
            this.Person = person;
            this.Password = password;
            this.PermissionLevel = permissionLevel;
        }
        #endregion

        /// <summary>
        /// Change the current password
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="newPassword"></param>
        public bool ChangePassword(string username,string password, string newPassword)
        {
            if (VerifyCredentials(username, password))
            {
                if (VerifyPasswordRequirements(newPassword))
                {
                    this.Password = newPassword;
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else 
            { 
                return false; 
            }
        }

        public bool ChangeUsername(string username, string password, string newUsername)
        {
            if (VerifyCredentials(username, password))
            {
                if (VerifyUsernameRequirements(newUsername))
                {
                    this.Username = newUsername;
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// Check username and password
        /// WARNING: NOT SECURE, FOR PROTOTYPE ONLY!!
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool VerifyCredentials(string username, string password) => password == this.Password && username == this.Username;

        /// <summary>
        /// Check password requirements:
        /// At least 8 chars in length
        /// WARNING: NOT SECURE, FOR PROTOTYPE ONLY!!
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private bool VerifyPasswordRequirements(string password) => password.Length > 7;

        /// <summary>
        /// Check username requirements:
        /// At least 6 chars in length
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private bool VerifyUsernameRequirements(string username) => username.Length > 5;

    }
}
