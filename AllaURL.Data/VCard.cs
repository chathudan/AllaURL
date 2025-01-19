using AllaURL.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllaURL.Data
{
    public abstract class VCard
    {
        public int Id { get; }

        public DateTime LastUpdatedAt { get; set; }

        public VcardType VcardType { get; }

        public string Identifier { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string Website { get; set; }

        public DateTime CreatedAt { get; set; }

        // Custom properties as key-value pairs
        [NotMapped]
        public Dictionary<string, string> CustomProperties { get; set; } = new();

        // Method to generate vCard format
        public abstract string GenerateVCard();

    }

    public class PersonVCard : VCard
    {
        public string Title { get; set; } // Mr. Mrs. Dr. etc 
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string JobTitle { get; set; } // Job title, e.g., "Software Engineer"

        public VcardType VcardType { get; private set; } = VcardType.Person;

        public override string GenerateVCard()
        {
            var vCardBuilder = new StringBuilder();

            vCardBuilder.AppendLine("BEGIN:VCARD");
            vCardBuilder.AppendLine("VERSION:3.0");
            vCardBuilder.AppendLine($"FN:{FirstName} {LastName}");
            vCardBuilder.AppendLine($"N:{LastName};{FirstName};;;");
            vCardBuilder.AppendLine($"TITLE:{Title}");
            vCardBuilder.AppendLine($"EMAIL;TYPE=INTERNET:{Email}");
            vCardBuilder.AppendLine($"TEL;TYPE=CELL:{Phone}");
            vCardBuilder.AppendLine($"URL:{Website}");
            vCardBuilder.AppendLine($"ADR;TYPE=HOME:;;{Address};;;;");

            // Include custom properties in the vCard
            foreach (var property in CustomProperties)
            {
                vCardBuilder.AppendLine($"{property.Key}:{property.Value}");
            }

            vCardBuilder.AppendLine("END:VCARD");

            return vCardBuilder.ToString();
        }
    }

    public class CompanyVCard : VCard
    {
        public string CompanyName { get; set; }

        public string CompanyNumber { get; set; } // ABN , DUNS 

        public VcardType VcardType { get; private set; } = VcardType.Company;

        public override string GenerateVCard()
        {
            var vCardBuilder = new StringBuilder();

            vCardBuilder.AppendLine("BEGIN:VCARD");
            vCardBuilder.AppendLine("VERSION:3.0");
            vCardBuilder.AppendLine($"ORG:{CompanyName}");
            vCardBuilder.AppendLine($"URL:{Website}");
            vCardBuilder.AppendLine($"EMAIL;TYPE=INTERNET:{Email}");
            vCardBuilder.AppendLine($"TEL;TYPE=WORK:{Phone}");
            vCardBuilder.AppendLine($"ADR;TYPE=WORK:;;{Address};;;;");

            // Include custom properties in the vCard
            foreach (var property in CustomProperties)
            {
                vCardBuilder.AppendLine($"{property.Key}:{property.Value}");
            }

            vCardBuilder.AppendLine("END:VCARD");

            return vCardBuilder.ToString();
        }
    }

    public enum VcardType
    {
        Person,
        Company
    }

}
