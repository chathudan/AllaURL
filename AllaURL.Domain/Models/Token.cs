using AllaURL.Common;
using AllaURL.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace AllaURL.Domain.Models
{
    public class Token 
    {
        [NotMapped]
        public int Id { get; set; }
        public string Identifier { get; set; }

        public bool IsActive { get; set; }

        public bool IsAllocated { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastUpdatedAt { get; set; }

        public TokenType TokenType { get; set; }

        public TokenData TokenData { get; set; }  // Reference to domain data (URL or VCard)
    }
}