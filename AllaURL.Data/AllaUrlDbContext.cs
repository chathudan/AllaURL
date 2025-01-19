using AllaURL.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AllaURL.Data;

public class AllaUrlDbContext : DbContext
{
    // Default constructor is removed because the parameterized one is used
    // public AllaUrlDbContext() {}

    public AllaUrlDbContext(DbContextOptions<AllaUrlDbContext> dbContextOptions) : base(dbContextOptions)
    { }
    
    public virtual DbSet<TokenEntity> TokenEntity { get; set; }
    
    public virtual DbSet<TokenDataEntity> TokenDataEntity { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TokenEntityConfiguration());
        modelBuilder.ApplyConfiguration(new TokenDataEntityConfiguration()); 

        
        modelBuilder.Entity<TokenEntity>(); 
        modelBuilder.Entity<TokenDataEntity>();



    }
}