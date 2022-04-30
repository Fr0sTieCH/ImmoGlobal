using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmoGlobalAdmin.ViewModel
{
    internal interface IHasSearchableContent
    {
        public void SearchContent(string searchString);
    }
}
