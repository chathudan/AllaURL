using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllaURL.Domain.Models
{
    public class CompanyVCardData : VCardData
    {
        public string CompanyName { get; set; }
        public string CompanyNumber { get; set; } // ABN, DUNS, etc.
    }
}
