using AllaURL.Data.Entities; 
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using JetBrains.Annotations;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AllaURL.Data;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using AllaURL.Common;
using AllaURL.Domain.Repositories;

namespace API.UnitTest.Repositories
{
    [TestSubject(typeof(TokenRepository))]
    public class TokenRepositoryTest
    {
        private readonly Mock<IDistributedCache> _cacheMock = new();
        private readonly Mock<AllaUrlDbContext> _contextMock = new();

        [Fact]
        public async Task GetByTokenAsync_ThrowsException_WhenTokenNotFound()
        {
            _contextMock.Setup(o => o.TokenEntity).Returns(DbSetMock(new List<TokenEntity>()).Object);
            var rep = new TokenRepository(_cacheMock.Object, _contextMock.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() => rep.GetByTokenAsync("Token"));
        }

        [Fact]
        public async Task GetByTokenAsync_ReturnsCorrectToken_WhenTokenExistsInDatabase()
        {
            var testTokenEntity = new TokenEntity
                { Identifier = "washia", TokenDataEntity = new TokenDataEntity { TokenType = TokenType.Url } };

            _contextMock.Setup(o => o.TokenEntity).Returns(DbSetMock(new List<TokenEntity> { testTokenEntity }).Object);

            var rep = new TokenRepository(_cacheMock.Object, _contextMock.Object);
            var result = await rep.GetByTokenAsync("washia");
            Assert.Equal("washia", result.Identifier);
        }

        [Fact]
        public async Task SaveAsync_ThrowsException_WhenTokenIsNull()
        {
            var repository = new TokenRepository(_cacheMock.Object, _contextMock.Object);
            await Assert.ThrowsAsync<ArgumentNullException>(() => repository.SaveAsync(null));
        }

        // Add more test methods for other cases

        private static Mock<DbSet<T>> DbSetMock<T>(IEnumerable<T> elements) where T : class, new()
        {
            var elementsAsQueryable = elements.AsQueryable();
            var dbSetMock = new Mock<DbSet<T>>();

            dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(elementsAsQueryable.Provider);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(elementsAsQueryable.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(elementsAsQueryable.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(elementsAsQueryable.GetEnumerator());

            return dbSetMock;
        }
        
        [Fact]
        public async Task SaveAsync_SuccessfullySavesToken_WhenTokenIsValid()
        {
            var testTokenEntity = new TokenEntity
            {
                Identifier = "washia", 
                IsActive = true,
                IsAllocated = true,
                TokenDataEntity = new TokenDataEntity
                {
                    TokenType = TokenType.Url,
                    TokenData = new UrlEntity
                    {
                        RedirectUrl = "https://washia.com.au",
                        CreatedAt = DateTime.UtcNow,
                        LastUpdatedAt = DateTime.UtcNow,
                    }
                },
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow,
                Type = TokenType.Url
                
            };

            _contextMock.Setup(o => o.TokenEntity).Returns(DbSetMock(new List<TokenEntity> { testTokenEntity }).Object);
var databaseMock = new Mock<DatabaseFacade>(_contextMock.Object);
_contextMock.Setup(m => m.Database).Returns(databaseMock.Object);
databaseMock.Setup(db => db.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                     .Returns(Task.FromResult(Mock.Of<IDbContextTransaction>()));
            _contextMock.Setup(o => o.SaveChangesAsync(default(CancellationToken))).ReturnsAsync(1);

var urlEntityList = new List<UrlEntity>
{
    new UrlEntity
    {
        RedirectUrl = "https://washia.com.au",
        CreatedAt = DateTime.UtcNow,
        LastUpdatedAt = DateTime.UtcNow
    }
};

_contextMock.Setup(o => o.UrlEntity).Returns(DbSetMock(urlEntityList).Object);
            var rep = new TokenRepository(_cacheMock.Object, _contextMock.Object);
            await rep.SaveAsync(testTokenEntity);
            _contextMock.Verify(o => o.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task SaveAsync_ThrowsException_WhenTokenIdentifierIsNull()
        {
            var testTokenEntity = new TokenEntity
                { Identifier = null, TokenDataEntity = new TokenDataEntity { TokenType = TokenType.Url } };

            var rep = new TokenRepository(_cacheMock.Object, _contextMock.Object);
            await Assert.ThrowsAsync<ArgumentNullException>(() => rep.SaveAsync(testTokenEntity));
        }
    }
}