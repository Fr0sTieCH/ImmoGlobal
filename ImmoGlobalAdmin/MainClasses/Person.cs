using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoGlobalAdmin.MainClasses
{/// <summary>
/// Represents a legal person or an juristic entity
/// </summary>
    public class Person : ImmoGlobalEntity
    {

        public int PersonID { get; private set; }
        public string Name { get; set; } = "";
        public string Prename { get; set; } = "";
        public Sex Sex { get; set; }
        public string Adress { get; set; } = "";
        public string Phone { get; set; } = "";
        public string EMail { get; set; } = "";
        public string VatNuber { get; set; } = "";
        public DateTime? Birthdate { get; set; }
        public string Note { get; set; } = "";
        public PersonType Type { get; set; } = PersonType.PrivatePerson;


        [NotMapped]
        public string IGID => PersonID.ToString("PE00000000");

        [NotMapped]
        public string FullName => $"{Name} {Prename}";

        [NotMapped]
        public string EMailCommand => $"mailto:{EMail}";

        [NotMapped]
        public string SetPersonTypeString
        {
            get => Enum.GetName(Type);
            set => Type =  Enum.Parse<PersonType>(value);
        }

        [NotMapped]
        public string SetSexString
        {
            get => Enum.GetName(Sex);
            set => Sex = Enum.Parse<Sex>(value);
        }




        public Person()
        {
        }

        public Person(bool enabled)
        {
            Enabled = enabled;
        }

        #region CONSTRUCTORS
        /// <summary>
        /// New juristic entitiy
        /// </summary>
        /// <param name="name">Name of the entity</param>
        /// <param name="adress">Adress of the entity</param>
        /// <param name="phone">Phonenumber of the entity</param>
        /// <param name="fax">Faxnumber of the entity</param>
        /// <param name="eMail">Email adress of the entity </param>
        /// <param name="vatNuber">VAT-Number of the Entity</param>
        /// <param name="note">Additional Info</param>
        public Person(string name, string adress, string phone, string eMail, string vatNuber, string note)
        {
            this.Enabled = true;
            this.Name = name;
            this.Adress = adress;
            this.Phone = phone;
            this.EMail = eMail;
            this.VatNuber = vatNuber;
            this.Note = note;
        }

        /// <summary>
        /// New Person
        /// </summary>
        /// <param name="name">Surname</param>
        /// <param name="prename">Prename</param>
        /// <param name="adress">Adress in fromat: Street, Number, Zip, City, State/Provence, Country</param>
        /// <param name="phone">Phonenumber</param>
        /// <param name="fax">Faxnumber</param>
        /// <param name="eMail"> Email adress</param>
        /// <param name="birthdate">Birthdate (can be NULL)</param>
        /// <param name="note">additional info</param>
        public Person(string name, string prename, string adress, string phone, string eMail, DateTime? birthdate, string note)
        {
            this.Enabled = true;
            this.Name = name;
            this.Prename = prename;
            this.Adress = adress;
            this.Phone = phone;
            this.EMail = eMail;
            this.Birthdate = birthdate;
            this.Note = note;
        }


        #endregion

       
    }
}
