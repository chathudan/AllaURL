using AllaURL.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AllaURL.API.UnitTest;

public class NFCTagEntityTests : TestBase
{
    public NFCTagEntityTests()
    {
      
    }
    
    [Fact]
    public void TestAddNFCTag()
    {
        // Arrange
        using (var context = CreateContext())
        {
            var nfcTagEntity = new TokenEntity
            {
               
                //Vcard = "https://washia.com.au",
                Identifier = "washia",
                CreatedAt = DateTime.UtcNow,
                //RedirectUrl = "https://washia.com.au"
            };

            // Act
            context.TokenEntity.Add(nfcTagEntity);
            context.SaveChanges();

            // Assert
            Assert.Equal(1, context.TokenEntity.Count(t => t.Identifier.Equals("washia")));
            Assert.Equal("washia", context.TokenEntity.Single().Identifier);
        }
    }
    
    [Fact]
    public void TestGetNFCTagByIdentifier()
    {
        // Arrange
        using (var context = CreateContext())
        {
            // Act
            var fetchedTagEntity = context.TokenEntity.Single(t => t.Identifier == "washia");

            // Assert
            Assert.NotNull(fetchedTagEntity);
            Assert.Equal("washia", fetchedTagEntity.Identifier);
        }
    }
    
    /*[Fact]
    public void DisposeData()
    {
        using (var context = new NFCDbContext(DbContextOptions))
        {
            context.Database.EnsureDeleted();
        }
    }*/
}