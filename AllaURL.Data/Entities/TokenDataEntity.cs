using System.ComponentModel.DataAnnotations.Schema;
using AllaURL.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AllaURL.Data.Entities;

[Table("TokenData")]
public class TokenDataEntity : IEntity
{
    public int Id { get; set; }

    public int TokenId { get; set; }

    public TokenType TokenType { get; set; }

    public string RedirectUrl { get; set; }

    // Navigation property to Token
    [ForeignKey("TokenId")]
    public virtual TokenEntity TokenEntity { get; set; }


}

public class TokenDataEntityConfiguration : IEntityTypeConfiguration<TokenDataEntity>
{
    public void Configure(EntityTypeBuilder<TokenDataEntity> builder)
    {
        builder.HasKey(td => td.Id);
        builder.Property(td => td.Id)
            .ValueGeneratedOnAdd();  // Set the Id to auto-generate

        builder.Property(td => td.TokenType)
            .IsRequired();  // TokenType is required

        builder.Property(td => td.RedirectUrl)
            .HasMaxLength(500)  // Set the maximum length for the URL field
            .IsRequired(false);  // Optional: make it nullable since it may not always have a URL


        // Configure the foreign key to Token entity
        builder.HasOne(td => td.TokenEntity)  // TokenData belongs to one Token
               .WithOne(t => t.TokenDataEntity)  // Token has one TokenDataEntity
               .HasForeignKey<TokenDataEntity>(td => td.TokenId)  // TokenData's TokenId is the foreign key
               .OnDelete(DeleteBehavior.Cascade);  // Optional: Cascade delete if TokenDataEntity is deleted


    }
}