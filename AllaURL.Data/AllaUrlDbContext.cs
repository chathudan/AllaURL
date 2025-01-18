using AllaURL.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AllaURL.Data;

public class AllaUrlDbContext : DbContext
{
    public AllaUrlDbContext()
    {
        
    }

    public AllaUrlDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
    { }
    
    public virtual DbSet<TokenEntity> TokenEntity { get; set; }
    
    public virtual DbSet<VCardEntity> VCardEntity { get; set; }
    
    public virtual DbSet<UrlEntity> UrlEntity { get; set; }
    
    public virtual DbSet<TokenDataEntity> TokenDataEntity { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TokenEntityConfiguration());
        modelBuilder.ApplyConfiguration(new TokenDataEntityConfiguration());
        modelBuilder.ApplyConfiguration(new UrlEntityConfiguration());
        modelBuilder.ApplyConfiguration(new VcardEntityConfiguration());

        
        modelBuilder.Entity<TokenEntity>();
        modelBuilder.Entity<UrlEntity>();
        modelBuilder.Entity<VCardEntity>();
        modelBuilder.Entity<TokenDataEntity>();
    }
}