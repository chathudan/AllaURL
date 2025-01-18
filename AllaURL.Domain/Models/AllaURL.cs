using AllaURL.Common;
using AllaURL.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllaURL.Domain.Models;

public class AllaURL
{
    public AllaURL() {}

    public Token Token { get; set; }

    // Constructor for creating Token with associated data based on the type
    public AllaURL(string identifier, TokenType type, string redirectUrl = null, VCardData vCardData = null)
    {
        Token = new Token
        {
            Identifier = identifier,
            TokenType = type,
            TokenData = CreateTokenData(type, redirectUrl, vCardData)
        };
    }

    // Method to create TokenData based on TokenType
    private ITokenData CreateTokenData(TokenType type, string redirectUrl, VCardData vCardData)
    {
        // Create TokenData based on the TokenType
        if (type == TokenType.Url)
        {
            return new UrlData
            {
                RedirectUrl = redirectUrl,
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow
            };
        }
        else if (type == TokenType.Vcard)
        {
            // Use either PersonVCardData or CompanyVCardData
            if (vCardData is PersonVCardData)
            {
                return new PersonVCardData
                {
                    Title = ((PersonVCardData)vCardData).Title,
                    FirstName = ((PersonVCardData)vCardData).FirstName,
                    LastName = ((PersonVCardData)vCardData).LastName,
                    JobTitle = ((PersonVCardData)vCardData).JobTitle,
                    Email = vCardData.Email,
                    Phone = vCardData.Phone,
                    Address = vCardData.Address,
                    Website = vCardData.Website,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow
                };
            }
            else if (vCardData is CompanyVCardData)
            {
                return new CompanyVCardData
                {
                    CompanyName = ((CompanyVCardData)vCardData).CompanyName,
                    CompanyNumber = ((CompanyVCardData)vCardData).CompanyNumber,
                    Email = vCardData.Email,
                    Phone = vCardData.Phone,
                    Address = vCardData.Address,
                    Website = vCardData.Website,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow
                };
            }
            else
            {
                throw new InvalidOperationException("vCard data must be of type PersonVCardData or CompanyVCardData.");
            }
        }

        throw new InvalidOperationException("Unsupported TokenType.");
    }
}

