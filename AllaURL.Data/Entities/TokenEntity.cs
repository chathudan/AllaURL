using System.ComponentModel.DataAnnotations.Schema;
using AllaURL.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders; 

namespace AllaURL.Data.Entities;

public class TokenEntity : IEntity
{
    public int Id { get; set; }

    public string Identifier { get; set; }

    public bool IsActive { get; set; }

    public bool IsAllocated { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime LastUpdatedAt { get; set; }

    public TokenType Type { get; set; } = TokenType.Url;

    // Direct reference to the associated TokenDataEntity (either UrlEntity or VCardEntity)
    public virtual TokenDataEntity TokenDataEntity { get; set; }
}

internal class TokenEntityConfiguration : IEntityTypeConfiguration<TokenEntity>
{
    public void Configure(EntityTypeBuilder<TokenEntity> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id)
            .ValueGeneratedOnAdd();

        builder.Property(t => t.Identifier)
            .HasMaxLength(100)
            .IsRequired();

        // Ensure the relationship with TokenDataEntity is properly configured
        //builder.HasOne(t => t.TokenDataEntity)  // One TokenEntity has one TokenDataEntity
        //    .WithOne()  // TokenDataEntity has one TokenEntity
        //    .HasForeignKey<TokenDataEntity>(td => td.TokenId)  // Foreign key from TokenDataEntity to TokenEntity
        //    .OnDelete(DeleteBehavior.Cascade);  // Cascade delete when TokenEntity is deleted

        builder.HasIndex(t => t.Identifier)
            .IsUnique();
    }
}



