
using AllaURL.Common;
using AllaURL.Data;
using AllaURL.Data.Entities; 
using AllaURL.Domain.Exceptions;
using AllaURL.Domain.Extensions;
using AllaURL.Domain.Interfaces; 
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Caching.Distributed;

namespace AllaURL.Domain.Repositories;


public class TokenRepository(IDistributedCache cache, AllaUrlDbContext context) : ITokenRepository
{

    public async Task<TokenEntity> GetByTokenAsync(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            throw new ArgumentException("Token cannot be null or whitespace.", nameof(token));
        }

        TokenEntity cachedData = null;

        try
        {
            cachedData = await cache.GetDataAsync(token);
        }
        catch (Exception ex)
        {
            // Log the exception and proceed with fallback logic
            Console.WriteLine($"Error fetching from Redis: {ex.Message}");
        }

        if (cachedData is not null)
        {
            return cachedData;
        }

        var tokenEntity = await FetchTokenEntityAsync(token)
                          ?? throw new TokenNotFoundException($"Token '{token}' not found.");

        //Attempt to save the data in cache again(assuming Redis becomes available)
        try
        {
            await cache.SetDataAsync(token, tokenEntity.ConvertToDomain());
        }
        catch (Exception ex)
        {
            // Log the cache saving failure but continue processing the request
            Console.WriteLine($"Error saving to Redis: {ex.Message}");
        }

        return tokenEntity;
    }

    private async Task<TokenEntity?> FetchTokenEntityAsync(string token)
    {
        var tokenEntity = await context.TokenEntity
                                   .Include(t => t.TokenDataEntity) 
                                   .AsNoTracking()
                                   .FirstOrDefaultAsync(t => t.Identifier == token);

        return tokenEntity;
    }

    private async Task<TokenDataEntity?> FetchTokenDataEntityAsync(int tokenId)
    {
        var token = await context.TokenDataEntity.AsNoTracking().Where(t => t.TokenId == tokenId).FirstOrDefaultAsync();
        return await context.TokenDataEntity.AsNoTracking().Where(t => t.TokenId == tokenId).FirstOrDefaultAsync();

    }

    public async Task SaveAsync(TokenEntity token)
    {
        if (token == null || token.TokenDataEntity == null)
        {
            throw new ArgumentNullException(nameof(token), "Token or TokenDataEntity cannot be null");
        }

        try
        {

            var existingEntity = await context.TokenEntity
                .FirstOrDefaultAsync(t => t.Identifier == token.Identifier);

            EntityEntry<TokenEntity> tokenEntity;
            
            if (existingEntity is not null)
            {
                token.Id = existingEntity.Id;
                tokenEntity =  context.TokenEntity.Update(token);
            }
            else
            {
                tokenEntity = await context.TokenEntity.AddAsync(token);
            }

            await context.SaveChangesAsync();

            // At this point, TokenEntity and its TokenDataEntity are both tracked
            // Now update TokenDataEntity with the correct foreign key
            token.TokenDataEntity.TokenId = tokenEntity.Entity.Id;  // Set the foreign key in TokenDataEntity

            // Update TokenDataEntity
            context.TokenDataEntity.Update(token.TokenDataEntity);

            // Save the TokenDataEntity changes
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new Exception("An unexpected error has occurred while processing the request.", e);
        }
    }
}