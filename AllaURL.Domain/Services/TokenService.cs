using AllaURL.Domain.Extensions;
using AllaURL.Domain.Interfaces;
using AllaURL.Domain.Models;
using AllaURL.Domain.Repositories;

namespace AllaURL.Domain.Services;

public class TokenService(ITokenRepository repository) : ITokenService
{
    public async Task<Token> GetByTokenAsync(string token)
    {
        var tokenEntity = await repository.GetByTokenAsync(token);

        return tokenEntity.ConvertToDomain();
    }

    public async Task SaveAsync(Token token)
    {
        await repository.SaveAsync(token.ConvertToEntity());
    }

}