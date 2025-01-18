using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllaURL.Domain.Models
{
    public class UrlData : ITokenData
    {
        public int Id { get; set; }
        public string RedirectUrl { get; set; }
        public DateTime CreatedAt { get; internal set; }
        public DateTime LastUpdatedAt { get; internal set; }
    }
}
