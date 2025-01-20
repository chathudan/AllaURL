using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace AllaURL.Identity.Models
{
    [Table("AspNetUsers")]
    public class ApplicationUser : IdentityUser
    {
    }

    public enum UserStatus : byte { Disabled = 0, Active = 1, Pending = 2, Migrate = 3 }
}
