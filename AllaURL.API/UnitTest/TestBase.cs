using Microsoft.EntityFrameworkCore;
using System;
using AllaURL.Data;
using AllaURL.Data.Entities;
using Xunit;

namespace AllaURL.API.UnitTest;

public abstract class TestBase : IDisposable
{
    protected DbContextOptions<AllaUrlDbContext> DbContextOptions { get; }

    public TestBase()
    {
        DbContextOptions = new DbContextOptionsBuilder<AllaUrlDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        using (var context = new AllaUrlDbContext(DbContextOptions))
        {
            context.Database.EnsureCreated();
            SeedData(context);
        }
    }

    protected virtual void SeedData(AllaUrlDbContext context)
    {
        context.TokenEntity.AddRange(new List<TokenEntity>
        {
            new TokenEntity
            {
                Id = 1,
                //Vcard = "https://washia.com.au",
                Identifier = "washia",
                CreatedAt = DateTime.UtcNow,
               // RedirectUrl = "https://washia.com.au" 
            },
            new TokenEntity
            {
                Id = 2,
                //Vcard = "https://wildnestvilla.com",
                Identifier = "wildnestvilla",
                CreatedAt = DateTime.UtcNow,
               // RedirectUrl = "https://wildnestvilla.com" 
            }
        });

        context.SaveChanges();
    }

    protected AllaUrlDbContext CreateContext() => new AllaUrlDbContext(DbContextOptions);

    public void Dispose()
    {
        using (var context = new AllaUrlDbContext(DbContextOptions))
        {
            context.Database.EnsureDeleted();
        }
    }
}
