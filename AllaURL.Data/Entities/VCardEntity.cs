using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AllaURL.Data.Entities;

public abstract class VCardEntity : ITokenEntity,  IEntity
{
    public int Id { get; }

    public DateTime LastUpdatedAt { get; set; }

    public VcardType VcardType { get; }
    
    public string Identifier { get; set; }
    
    public string Email { get; set; }
    
    public string Phone { get; set; }
    
    public string Address { get; set; }
    
    public string Website { get; set; }
    
    /*public string LinkedIn { get; set; }
    public string Facebook { get; set; }
    public string X { get; set; }
    public string Instergram { get; set; } 
    public string MapLocationUrl { get; set; }*/
    
    public DateTime CreatedAt { get; set; }
    
    // Custom properties as key-value pairs
    [NotMapped]
    public Dictionary<string, string> CustomProperties { get; set; } = new();

    // Method to generate vCard format
    public abstract string GenerateVCard();
    
}

public class PersonVCardEntity : VCardEntity
{
    public string Title { get; set; } // Mr. Mrs. Dr. etc 
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public string JobTitle { get; set; } // Job title, e.g., "Software Engineer"

    public  VcardType VcardType { get; private set; } = VcardType.Person;

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

public class CompanyVCardEntity : VCardEntity
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


internal class VcardEntityConfiguration : IEntityTypeConfiguration<VCardEntity>
{
    public void Configure(EntityTypeBuilder<VCardEntity> builder)
    {
        builder.HasKey(v => v.Id);
        builder.Property(v => v.Id)
            .ValueGeneratedOnAdd();


        builder.Property(v => v.Identifier)
           .HasMaxLength(100)
           .IsRequired();

        builder.Property(v => v.Email)
            .HasMaxLength(100);

        builder.Property(v => v.Phone)
            .HasMaxLength(50);

        builder.Property(v => v.Address)
            .HasMaxLength(255);

        builder.Property(v => v.Website)
            .HasMaxLength(255);

        //builder.Property(v => v.Title)
        //    .HasMaxLength(50);

        //builder.Property(v => v.FirstName)
        //    .HasMaxLength(100);

        //builder.Property(v => v.LastName)
        //    .HasMaxLength(100);

        //builder.Property(v => v.JobTitle)
        //    .HasMaxLength(100);

        //builder.Property(v => v.CompanyName)
        //    .HasMaxLength(255);

        //builder.Property(v => v.CompanyNumber)
        //    .HasMaxLength(50);

        builder.HasDiscriminator<VcardType>("VcardType")
            .HasValue<PersonVCardEntity>(VcardType.Person)
            .HasValue<CompanyVCardEntity>(VcardType.Company);

        builder.Ignore(v => v.CustomProperties);
        builder.HasIndex(v => v.Email).IsUnique();
    }
}