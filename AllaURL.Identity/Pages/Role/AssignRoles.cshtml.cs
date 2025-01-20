using AllaURL.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AllaURL.Identity.Pages.Role
{
    public class AssignRolesModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AssignRolesModel(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [BindProperty]
        public string UserId { get; set; }

        [BindProperty]
        public string UserName { get; set; }

        [BindProperty]
        public string RoleName { get; set; }

        public string Message { get; set; }

        public List<string> AssignedRoles { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            UserId = user.Id;
            UserName = user.UserName;
            AssignedRoles = new List<string>(await _userManager.GetRolesAsync(user));

            return Page();
        }

        public async Task<IActionResult> OnPostAssignRoleAsync()
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                Message = "User not found.";
                return Page();
            }

            var roleExists = await _roleManager.RoleExistsAsync(RoleName);
            if (!roleExists)
            {
                Message = "Role not found.";
                return Page();
            }

            var result = await _userManager.AddToRoleAsync(user, RoleName);
            if (result.Succeeded)
            {
                Message = "Role assigned successfully.";
            }
            else
            {
                Message = "Error assigning role.";
            }

            AssignedRoles = new List<string>(await _userManager.GetRolesAsync(user));
            return Page();
        }

        public async Task<IActionResult> OnPostRemoveRoleAsync()
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                Message = "User not found.";
                return Page();
            }

            var result = await _userManager.RemoveFromRoleAsync(user, RoleName);
            if (result.Succeeded)
            {
                Message = "Role removed successfully.";
            }
            else
            {
                Message = "Error removing role.";
            }

            AssignedRoles = new List<string>(await _userManager.GetRolesAsync(user));
            return Page();
        }
    }
}