
using AllaURL.Data.Entities;

namespace AllaURL.Domain.Repositories;

public interface ITokenRepository
{
    Task<TokenEntity> GetByTokenAsync(string token);
    Task SaveAsync(TokenEntity tokenEntity);
    // Add any other needed methods...
}