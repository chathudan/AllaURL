using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllaURL.Domain.Models
{
    public class PersonVCardData : VCardData
    {
        public string Title { get; set; } // Mr., Mrs., Dr.
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; } // Software Engineer, etc.
    }
}
