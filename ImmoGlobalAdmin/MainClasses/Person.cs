using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoGlobalAdmin.MainClasses
{
    /// <summary>
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

        #region CONSTRUCTORS
        public Person()//used by EntityFramework
        {
        }

        public Person(bool enabled)
        {
            Enabled = enabled;
        }
        #endregion

        #region PUBLIC GETSET
        [NotMapped]
        public string IGID => PersonID.ToString("PE 00000000");

        [NotMapped]
        public string FullName => $"{Name} {Prename}";

        [NotMapped]
        public string EMailCommand => $"mailto:{EMail}";

        [NotMapped]
        public string SetPersonTypeString //returns and setts the type by converting a string
        {
            get => Enum.GetName(Type) ?? "";
            set => Type = Enum.Parse<PersonType>(value);
        }

        [NotMapped]
        public string SetSexString //returns and setts the type by converting a string
        {
            get => Enum.GetName(Sex) ?? "";
            set => Sex = Enum.Parse<Sex>(value);
        }
        #endregion







    }
}
