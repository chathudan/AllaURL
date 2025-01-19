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

           
            return Ok(tokenData); 
        }

    }
}


