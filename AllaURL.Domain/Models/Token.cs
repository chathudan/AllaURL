using AllaURL.Common;
using AllaURL.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace AllaURL.Domain.Models
{
    public class Token : ITokenData
    {
        [NotMapped]
        public int Id { get; set; }
        public string Identifier { get; set; }
        public TokenType TokenType { get; set; }

        public ITokenData TokenData { get; set; }  // Reference to domain data (URL or VCard)
    }
}