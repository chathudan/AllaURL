using System.ComponentModel.DataAnnotations.Schema;
using AllaURL.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders; 

namespace AllaURL.Data.Entities;

[Table("Tokens")]
public class TokenEntity : IEntity
{
    public int Id { get; set; }

    public string Identifier { get; set; }

    public bool IsActive { get; set; }

    public bool IsAllocated { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime LastUpdatedAt { get; set; }

    public TokenType TokenType { get; set; } = TokenType.Url;

    // Direct reference to the associated TokenDataEntity (either UrlEntity or VCardEntity)
    public virtual TokenDataEntity TokenDataEntity { get; set; }
}

internal class TokenEntityConfiguration : IEntityTypeConfiguration<TokenEntity>
{
    public void Configure(EntityTypeBuilder<TokenEntity> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id)
            .ValueGeneratedOnAdd();  // Set the Id to auto-generate

        builder.Property(t => t.Identifier)
            .HasMaxLength(100)
            .IsRequired();  // Identifier should be required
    }

}



