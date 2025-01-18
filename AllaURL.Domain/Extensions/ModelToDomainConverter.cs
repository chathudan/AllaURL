using AllaURL.Common;
using AllaURL.Data.Entities;
using AllaURL.Domain.Models;

namespace AllaURL.Domain.Extensions
{
    public static class ModelToDomainConverter
    {
        // Convert TokenEntity to Token (Domain Model)
        public static Token ConvertToDomain(this Data.Entities.TokenEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            ITokenData tokenData = entity.TokenDataEntity.TokenType == TokenType.Url
                ? new UrlData
                {
                    Id = entity.TokenDataEntity.TokenDataId,
                    RedirectUrl = ((UrlEntity)entity.TokenDataEntity.TokenData).RedirectUrl
                }
                : entity.TokenDataEntity.TokenType == TokenType.Vcard && entity.TokenDataEntity.TokenData is PersonVCardEntity
                ? new PersonVCardData
                {
                    Id = entity.TokenDataEntity.TokenDataId,
                    Identifier = ((VCardEntity)entity.TokenDataEntity.TokenData).Identifier,
                    Email = ((VCardEntity)entity.TokenDataEntity.TokenData).Email,
                    Phone = ((VCardEntity)entity.TokenDataEntity.TokenData).Phone,
                    Address = ((VCardEntity)entity.TokenDataEntity.TokenData).Address,
                    Website = ((VCardEntity)entity.TokenDataEntity.TokenData).Website
                }
                : new CompanyVCardData
                {
                    Id = entity.TokenDataEntity.TokenDataId,
                    Identifier = ((VCardEntity)entity.TokenDataEntity.TokenData).Identifier,
                    Email = ((VCardEntity)entity.TokenDataEntity.TokenData).Email,
                    Phone = ((VCardEntity)entity.TokenDataEntity.TokenData).Phone,
                    Address = ((VCardEntity)entity.TokenDataEntity.TokenData).Address,
                    Website = ((VCardEntity)entity.TokenDataEntity.TokenData).Website
                };

            return new Token
            {
                Id = entity.Id,
                Identifier = entity.Identifier,
                TokenType = entity.TokenDataEntity.TokenType,
                TokenData = tokenData
            };
        }

        // Convert Token (Domain Model) to TokenEntity (Persistence Model)
        public static Data.Entities.TokenEntity ConvertToEntity(this Token token)
        {
            if (token == null)
            {
                return null;
            }

            var tokenDataEntity = new TokenDataEntity
            {
                TokenType = token.TokenType,
                TokenDataId = token.TokenData.Id
            };

            if (token.TokenType == TokenType.Url)
            {
                var urlEntity = (UrlData)token.TokenData;
                tokenDataEntity.TokenData = new UrlEntity
                {
                    RedirectUrl = urlEntity.RedirectUrl,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow
                };
            }
            else if (token.TokenType == TokenType.Vcard)
            {
                if (token.TokenData is PersonVCardData personVCardData)
                {
                    tokenDataEntity.TokenData = new PersonVCardEntity
                    {
                        Identifier = token.Identifier,
                        Title = personVCardData.Title,
                        FirstName = personVCardData.FirstName,
                        LastName = personVCardData.LastName,
                        JobTitle = personVCardData.JobTitle, 
                        Email = personVCardData.Email,
                        Phone = personVCardData.Phone,
                        Address = personVCardData.Address,
                        Website = personVCardData.Website,
                        CreatedAt = DateTime.UtcNow,
                        LastUpdatedAt = DateTime.UtcNow
                    };
                }
                else
                {
                    var companyVCardData = (CompanyVCardData)token.TokenData;
                    tokenDataEntity.TokenData = new CompanyVCardEntity
                    {
                        Identifier = companyVCardData.Identifier,
                        Email = companyVCardData.Email,
                        Phone = companyVCardData.Phone,
                        Address = companyVCardData.Address,
                        Website = companyVCardData.Website,
                        CreatedAt = DateTime.UtcNow,
                        LastUpdatedAt = DateTime.UtcNow
                    };

                }
            }

            return new Data.Entities.TokenEntity
            {
                Id = token.Id,
                Identifier = token.Identifier,
                TokenDataEntity = tokenDataEntity
            };
        }
    }
}
