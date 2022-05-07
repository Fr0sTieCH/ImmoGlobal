using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoGlobalAdmin.MainClasses
{
    public abstract class ImmoGlobalEntity
    {
        public virtual bool Enabled { get; protected set; }
        public virtual string ReasonForDeleting { get; protected set; } = "";
        public virtual bool Locked { get; protected set; }


        /// <summary>
        /// Deleting an Entity only sets Enabled to false, that way we dont loose references
        /// </summary>
        /// <param name="reason"></param>
        public virtual void Delete(string reason)
        {
            Enabled = false;
            ReasonForDeleting = reason;
        }

        public virtual void Lock()
        {
            Locked = true;
        }
    }
}
