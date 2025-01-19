using AllaURL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllaURL.Domain.Models
{
    public interface ITokenData
    {
        int Id { get; }
    }

    public class TokenData 
    {
        public int Id { get; set; }

        public int TokenId { get; set; }

        public TokenType TokenType { get; set; }

        public string RedirectUrl { get; set; }

    }
}
