using AllaURL.Common;
using AllaURL.Data.Entities;
using AllaURL.Domain.Models;

namespace AllaURL.Domain.Extensions
{
    public static class ModelToDomainConverter
    {

        public static Token ConvertToDomain(this Data.Entities.TokenEntity entity)
        {
            return new Token
            {
                Id = entity.Id,
                Identifier = entity.Identifier,
                TokenType = entity.TokenDataEntity.TokenType,
                TokenData = entity.TokenDataEntity.ConvertToDomain(),
            };
        }

        public static Data.Entities.TokenEntity ConvertToEntity(this Token token)
        {
            return new Data.Entities.TokenEntity
            {
                Identifier = token.Identifier,
                IsActive = token.IsActive,
                IsAllocated = token.IsAllocated,
                CreatedAt = token.CreatedAt,
                LastUpdatedAt = token.LastUpdatedAt,
                TokenDataEntity = (token.TokenData as TokenData)?.ConvertToEntity()
            };
        }

        public static TokenData ConvertToDomain(this Data.Entities.TokenDataEntity entity)
        {
           return new TokenData
           {
               Id = entity.Id,
               TokenId = entity.TokenId,
               TokenType = entity.TokenType,
               RedirectUrl = entity.RedirectUrl
           };
        }

        public static Data.Entities.TokenDataEntity ConvertToEntity(this TokenData token)
        {
            return new Data.Entities.TokenDataEntity
            {
                TokenId = token.TokenId,
                TokenType = token.TokenType,
                RedirectUrl = token.RedirectUrl
            };
        }

        public static Data.Entities.TokenDataEntity ConvertToEntityWithTokenDataId(this TokenData token)
        {
            return new Data.Entities.TokenDataEntity
            {
                Id = token.Id,
                TokenId = token.TokenId,
                TokenType = token.TokenType,
                RedirectUrl = token.RedirectUrl
            };
        }
    }
}
