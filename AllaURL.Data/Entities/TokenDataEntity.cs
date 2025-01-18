using System.ComponentModel.DataAnnotations.Schema;
using AllaURL.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AllaURL.Data.Entities;

public class TokenDataEntity : IEntity
{
    public int Id { get; set; }

    public int TokenId { get; set; }

    public TokenType TokenType { get; set; }

    // Foreign key to either UrlEntity or VCardEntity
    public int TokenDataId { get; set; }

    // Navigation property to the actual token data (UrlEntity or VCardEntity)
    [NotMapped]
    public virtual ITokenEntity TokenData { get; set; }
}

public class TokenDataEntityConfiguration : IEntityTypeConfiguration<TokenDataEntity>
{
    public void Configure(EntityTypeBuilder<TokenDataEntity> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id)
            .ValueGeneratedOnAdd(); 

        // TokenDataEntity is related to TokenEntity and stores foreign key (TokenIdentifier)
        builder.HasOne<TokenEntity>()
            .WithOne()
            .HasForeignKey<TokenEntity>(t => t.Id)
            .OnDelete(DeleteBehavior.Cascade);

        // TokenDataEntity stores the foreign key (TokenDataId) to either UrlEntity or VCardEntity
        builder.Property(t => t.TokenDataId)
            .IsRequired(); // Ensure the token data is not nullable

        // Configure the Discriminator (could be used if polymorphism is in play)
        builder.Property(t => t.TokenType)
            .IsRequired();

        // Optionally, configure the TokenType enum for storage as integer in the DB
        builder.Property(t => t.TokenType)
            .HasConversion<int>(); // Converts the TokenType enum to an integer (default approach)

        // Configuring the relationship to UrlEntity or VCardEntity based on TokenType
        builder.HasOne<UrlEntity>()
            .WithMany()
            .HasForeignKey(t => t.TokenDataId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);

        builder.HasOne<VCardEntity>()
           .WithMany()
           .HasForeignKey(t => t.TokenDataId)
           .OnDelete(DeleteBehavior.Cascade)
           .IsRequired(false);



        // Configure indexes if needed
        builder.HasIndex(t => new { t.Id, t.TokenDataId }).IsUnique();

        // Configure the discriminator column
        //builder.HasDiscriminator<int>("TokenType")
        //    .HasValue<UrlEntity>((int)TokenType.Url)
        //    .HasValue<VCardEntity>((int)TokenType.Vcard);
    }
}