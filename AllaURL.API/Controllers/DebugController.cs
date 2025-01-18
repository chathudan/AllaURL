using AllaURL.Common;
using AllaURL.Data;
using AllaURL.Data.Entities;
using AllaURL.Domain.Extensions;
using AllaURL.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllaURL.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DebugController : ControllerBase
    {
        private readonly AllaUrlDbContext _context;

        public DebugController(AllaUrlDbContext context)
        {
            _context = context;
        }

        [HttpGet("tokens")]
        public async Task<ActionResult<List<TokenEntity>>> GetTokens()
        { 

            var tokens = await _context.TokenEntity
                .Include(t => t.TokenDataEntity)  // Include the TokenDataEntity
                .ToListAsync();

            foreach (var token in tokens)
            {
                // Manually load the correct type of TokenDataEntity based on TokenType
                if (token.TokenDataEntity.TokenType == TokenType.Url)
                {
                    var urlEntity = await _context.UrlEntity
                        .FirstOrDefaultAsync(u => u.Id == token.TokenDataEntity.TokenDataId);
                    token.TokenDataEntity.TokenData = urlEntity;  // Manually populate the TokenData with UrlEntity
                }
                else if (token.TokenDataEntity.TokenType == TokenType.Vcard)
                {
                    var vcardEntity = await _context.VCardEntity
                        .FirstOrDefaultAsync(v => v.Id == token.TokenDataEntity.TokenId);
                    token.TokenDataEntity.TokenData = vcardEntity;  // Manually populate the TokenData with VCardEntity
                }

               
            }

            return Ok(tokens);
        }

        [HttpGet("token/{tokenId}")]
        public async Task<ActionResult> GetTokenById(int tokenId)
        {
            var token = await _context.TokenDataEntity.AsNoTracking().Where(t => t.TokenId == tokenId).FirstOrDefaultAsync();

            return Ok(token);
        }

        // Get all TokenDataEntities, which include UrlEntities and VCardEntities
        [HttpGet("token-data")]
        public async Task<ActionResult<IEnumerable<TokenDataEntity>>> GetTokenData()
        {
            var tokenData = await _context.TokenDataEntity.ToListAsync();  // Fetch all TokenDataEntity records

            // Conditionally load related data based on TokenType
            foreach (var token in tokenData)
            {
                if (token.TokenType == TokenType.Url)
                {
                    token.TokenData = await _context.UrlEntity
                        .Where(u => u.Id == token.TokenDataId)
                        .FirstOrDefaultAsync(); // Load related UrlEntity based on TokenDataId
                }
                else if (token.TokenType == TokenType.Vcard)
                {
                    token.TokenData = await _context.VCardEntity
                        .Where(v => v.Id == token.TokenDataId)
                        .FirstOrDefaultAsync(); // Load related VCardEntity based on TokenDataId
                }
            }
            return Ok(tokenData); 
        }

        [HttpGet("urls")]
        public async Task<ActionResult<IEnumerable<UrlEntity>>> GetUrls()
        {
            var urls = await _context.UrlEntity.ToListAsync();  // Fetch all UrlEntity records
            return Ok(urls);
        }

        // Get all VCards from VCardEntity
        [HttpGet("vcrds")]
        public async Task<ActionResult<IEnumerable<VCardEntity>>> GetVCards()
        {
            var vcrds = await _context.VCardEntity.ToListAsync();  // Fetch all VCardEntity records
            return Ok(vcrds);
        }
    }
}


