using AllaURL.Common;
using AllaURL.Data; 
using AllaURL.Data.Extensions;
using AllaURL.Domain.Extensions;
using AllaURL.Domain.Interfaces;
using AllaURL.Domain.Models;
using AllaURL.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace AllaURL.API.Controllers;

[ApiController]
public class URLController(ITokenService tokenService) : ControllerBase
{
    [HttpGet]
    [Route("/f/{token}")] 
    //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> ForwardUrl(string token)
    {
        if (string.IsNullOrWhiteSpace(token)) return BadRequest();

        var ipAddress = Helper.GetClientIp(HttpContext);

        return await PerformTokenRedirect(token, ipAddress);
    }

    [HttpPost]
    [Route("/Create")]
    public async Task<IActionResult> CreateTokenData([FromBody] AllaURL.Domain.Models.AllaURL allaURL)
    {
        if (allaURL == null)
        {
            return BadRequest("Token data is required.");
        }

        // Validate token model
        if (string.IsNullOrWhiteSpace(allaURL.Token.Identifier) ||
            (allaURL.Token.TokenType == TokenType.Url && string.IsNullOrWhiteSpace(((UrlData)allaURL.Token.TokenData)?.RedirectUrl)))
        {
            return BadRequest("Token Identifier and Redirect URL are required.");
        }

        // Save the token and its associated data (UrlEntity or VCardEntity)
        await tokenService.SaveAsync(allaURL.Token);

        return Ok(new { message = "Token created successfully", allaURL.Token.Identifier });
    }

    private async Task<IActionResult> PerformTokenRedirect(string token, string ipAddress)
    {
        var isValid = true; // TODO: Add actual validation logic for the token
        if (!isValid)
        {
            return BadRequest("Invalid token.");
        }

        var tokenEntity = await tokenService.GetByTokenAsync(token);

        if (tokenEntity == null || tokenEntity.TokenData == null)
        {
            return NotFound("Token not found.");
        }

        // Check the type of token (URL or vCard)
        if (tokenEntity.TokenType == TokenType.Url)
        {
            // Redirect to the stored URL
            if (tokenEntity.TokenData is UrlData urlData)
            {
                return Redirect(urlData.RedirectUrl);
            }
            else
            {
                return BadRequest("Invalid URL data.");
            }
        }
        else if (tokenEntity.TokenType == TokenType.Vcard)
        {
            // Generate vCard data (either person or company)
            var vCardData = tokenEntity.TokenData as VCardData;
            if (vCardData != null)
            {
                var vCardString = GenerateVCard(vCardData);

                // Return the vCard as a downloadable file
                var vCardBytes = Encoding.UTF8.GetBytes(vCardString);
                return File(vCardBytes, "text/vcard", $"{tokenEntity.Identifier}.vcf");
            }
            else
            {
                return BadRequest("Invalid vCard data.");
            }
        }

        return BadRequest("Invalid token type.");
    }



    private string GenerateVCard(VCardData vCardData)
    {
        var vCardBuilder = new StringBuilder();
        vCardBuilder.AppendLine("BEGIN:VCARD");
        vCardBuilder.AppendLine("VERSION:3.0");

        // Common fields
        vCardBuilder.AppendLine($"FN:{vCardData.Identifier}");
        vCardBuilder.AppendLine($"EMAIL:{vCardData.Email}");
        vCardBuilder.AppendLine($"TEL:{vCardData.Phone}");
        vCardBuilder.AppendLine($"ADR:{vCardData.Address}");
        vCardBuilder.AppendLine($"URL:{vCardData.Website}");

        // Specific fields for person or company
        if (vCardData is PersonVCardData person)
        {
            vCardBuilder.AppendLine($"N:{person.LastName};{person.FirstName};;;");
            vCardBuilder.AppendLine($"TITLE:{person.Title}");
            vCardBuilder.AppendLine($"JOBTITLE:{person.JobTitle}");
        }
        else if (vCardData is CompanyVCardData company)
        {
            vCardBuilder.AppendLine($"ORG:{company.CompanyName}");
            vCardBuilder.AppendLine($"COMPANYNUMBER:{company.CompanyNumber}");
        }

        vCardBuilder.AppendLine("END:VCARD");

        return vCardBuilder.ToString();
    }
}