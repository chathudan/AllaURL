using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AllaURL.Data.Entities;

public class UrlEntity : ITokenEntity,  IEntity
{
    public int Id { get; }
    
    //public int TokenId { get; }
    
    public string RedirectUrl { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime LastUpdatedAt { get; set; }

}

internal class UrlEntityConfiguration : IEntityTypeConfiguration<UrlEntity>
{
    public void Configure(EntityTypeBuilder<UrlEntity> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
            .ValueGeneratedOnAdd();


        builder.Property(u => u.RedirectUrl)
           .HasMaxLength(500)
           .IsRequired();

        //builder.Property(u => u.CreatedAt)
        //    .HasDefaultValueSql("GETDATE()");

        //builder.Property(u => u.LastUpdatedAt)
        //    .HasDefaultValueSql("GETDATE()");

        builder.HasIndex(u => u.RedirectUrl)
            .IsUnique();
    }
}