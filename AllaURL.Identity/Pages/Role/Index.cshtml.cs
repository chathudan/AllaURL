using AllaURL.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AllaURL.Identity.Pages.Role
{
    public class IndexModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [BindProperty]
        public string RoleName { get; set; }

        [BindProperty]
        public string OldRoleName { get; set; }

        [BindProperty]
        public string NewRoleName { get; set; }

        [BindProperty]
        public string DeleteRoleName { get; set; }

        public string Message { get; set; }

        public List<IdentityRole> Roles { get; set; }
        public List<UserWithRoles> Users { get; set; }

        public async Task OnGetAsync()
        {
            Roles = new List<IdentityRole>(await _roleManager.Roles.AsNoTracking().ToListAsync());
            Users = new List<UserWithRoles>();

            var users = await _userManager.Users.AsNoTracking().ToListAsync();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                Users.Add(new UserWithRoles
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Roles = roles.ToList()
                });
            }
        }

        public async Task<IActionResult> OnPostCreateRoleAsync()
        {
            if (string.IsNullOrWhiteSpace(RoleName))
            {
                Message = "Role name is required.";
                await OnGetAsync();
                return Page();
            }

            var roleExists = await _roleManager.RoleExistsAsync(RoleName);
            if (roleExists)
            {
                Message = "Role already exists.";
                await OnGetAsync();
                return Page();
            }

            var result = await _roleManager.CreateAsync(new IdentityRole(RoleName));
            if (result.Succeeded)
            {
                Message = "Role created successfully.";
            }
            else
            {
                Message = "Error creating role.";
            }

            await OnGetAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateRoleAsync()
        {
            if (string.IsNullOrWhiteSpace(OldRoleName) || string.IsNullOrWhiteSpace(NewRoleName))
            {
                Message = "Both old and new role names are required.";
                await OnGetAsync();
                return Page();
            }

            var role = await _roleManager.FindByNameAsync(OldRoleName);
            if (role == null)
            {
                Message = "Role not found.";
                await OnGetAsync();
                return Page();
            }

            role.Name = NewRoleName;
            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                Message = "Role updated successfully.";
            }
            else
            {
                Message = "Error updating role.";
            }

            await OnGetAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteRoleAsync()
        {
            if (string.IsNullOrWhiteSpace(DeleteRoleName))
            {
                Message = "Role name is required.";
                await OnGetAsync();
                return Page();
            }

            var role = await _roleManager.FindByNameAsync(DeleteRoleName);
            if (role == null)
            {
                Message = "Role not found.";
                await OnGetAsync();
                return Page();
            }

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                Message = "Role deleted successfully.";
            }
            else
            {
                Message = "Error deleting role.";
            }

            await OnGetAsync();
            return Page();
        }
    }

    public class UserWithRoles
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
    }
}