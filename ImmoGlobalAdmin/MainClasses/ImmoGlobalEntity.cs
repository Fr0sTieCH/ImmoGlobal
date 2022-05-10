using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoGlobalAdmin.MainClasses
{
    /// <summary>
    /// every entity that can be deleted or locked inherits from this class
    /// </summary>
    public abstract class ImmoGlobalEntity
    {
        public bool Enabled { get; protected set; }
        public string ReasonForDeleting { get; protected set; } = "";
        public bool Locked { get; protected set; }

        
        public void Delete(string Reason) => DeleteLogic(Reason);
        /// <summary>
        /// Deleting an Entity only sets Enabled to false, that way we dont loose references
        /// </summary>
        /// <param name="reason"></param>
        protected virtual void DeleteLogic(string reason)
        {
            Enabled = false;
            ReasonForDeleting = reason;
        }

        /// <summary>
        /// Sets Lock = True used to prevent unwanted changes
        /// </summary>
        public void Lock()
        {
            Locked = true;
        }
    }
}
