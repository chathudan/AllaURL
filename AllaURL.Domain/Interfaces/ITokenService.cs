
using AllaURL.Domain.Models;

namespace AllaURL.Domain.Interfaces;

public interface ITokenService
{
    Task<Token> GetByTokenAsync(string token);
    Task SaveAsync(Token tokenEntity);
    // Add any other necessary methods...
}