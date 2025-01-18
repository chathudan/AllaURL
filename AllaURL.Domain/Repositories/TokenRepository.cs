
using AllaURL.Common;
using AllaURL.Data;
using AllaURL.Data.Entities;
using AllaURL.Data.Extensions;
using AllaURL.Domain.Exceptions;
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

        tokenEntity.TokenDataEntity = await FetchTokenDataEntityAsync(tokenEntity.Id, tokenEntity.Type)
                                      ?? throw new TokenNotFoundException($"Token '{token}' data not found.");

        tokenEntity.TokenDataEntity.TokenData =
            await FetchSpecificTokenDataAsync(tokenEntity.TokenDataEntity.TokenDataId, tokenEntity.Type)
            ?? throw new TokenNotFoundException($"Token data not found for: {token}");

        // Attempt to save the data in cache again (assuming Redis becomes available)
        try
        {
            await cache.SetDataAsync(token, tokenEntity);
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
        return await context.TokenEntity.AsNoTracking().FirstOrDefaultAsync(t => t.Identifier == token);
    }

    private async Task<TokenDataEntity?> FetchTokenDataEntityAsync(int tokenId, TokenType tokenType)
    {
        var token = await context.TokenDataEntity.AsNoTracking().Where(t => t.TokenId == tokenId).FirstOrDefaultAsync();
        return await context.TokenDataEntity.AsNoTracking().Where(t => t.TokenId == tokenId).FirstOrDefaultAsync();

    }

    private async Task<ITokenEntity?> FetchSpecificTokenDataAsync(int tokenDataId, TokenType tokenType)
    {
        Task<ITokenEntity?> task = tokenType switch
        {
            TokenType.Vcard => FetchVCardEntityAsync(tokenDataId),
            TokenType.Url => FetchUrlEntityAsync(tokenDataId),
            _ => Task.FromResult<ITokenEntity?>(null)
        };

        return await task;
    }

    private async Task<ITokenEntity?> FetchVCardEntityAsync(int vCardId)
    {
        return await context.VCardEntity.FirstOrDefaultAsync(v => v.Id == vCardId);
    }

    private async Task<ITokenEntity?> FetchUrlEntityAsync(int urlId)
    {
        return await context.UrlEntity.FirstOrDefaultAsync(u => u.Id == urlId);
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

            if (token.TokenDataEntity.TokenType == TokenType.Vcard)
            {
                var vcardEntity = token.TokenDataEntity.TokenData as VCardEntity;
                 
                if (vcardEntity.VcardType == VcardType.Person)
                    vcardEntity = vcardEntity as PersonVCardEntity;
                else
                    vcardEntity = vcardEntity as CompanyVCardEntity;

                context.VCardEntity.Add(vcardEntity);
                await context.SaveChangesAsync();
                token.TokenDataEntity.TokenType = TokenType.Vcard;
                token.TokenDataEntity.TokenDataId = vcardEntity.Id;
            }
            else if (token.TokenDataEntity.TokenType == TokenType.Url)
            {
                var urlEntity = (UrlEntity)token.TokenDataEntity.TokenData;
                context.UrlEntity.Add(urlEntity);
                await context.SaveChangesAsync();
                token.TokenDataEntity.TokenType = TokenType.Url;
                token.TokenDataEntity.TokenDataId = urlEntity.Id;
            }
             
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