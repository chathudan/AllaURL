using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AllaURL.Identity.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        // Create a new role
        [HttpPost("create")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                return BadRequest("Role name is required.");
            }

            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (roleExists)
            {
                return BadRequest("Role already exists.");
            }

            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
            if (result.Succeeded)
            {
                return Ok("Role created successfully.");
            }

            return BadRequest(result.Errors);
        }

        // Update an existing role
        [HttpPost("update")]
        public async Task<IActionResult> UpdateRole(string oldRoleName, string newRoleName)
        {
            if (string.IsNullOrWhiteSpace(oldRoleName) || string.IsNullOrWhiteSpace(newRoleName))
            {
                return BadRequest("Both old and new role names are required.");
            }

            var role = await _roleManager.FindByNameAsync(oldRoleName);
            if (role == null)
            {
                return NotFound("Role not found.");
            }

            role.Name = newRoleName;
            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                return Ok("Role updated successfully.");
            }

            return BadRequest(result.Errors);
        }

        // Delete an existing role
        [HttpPost("delete")]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                return BadRequest("Role name is required.");
            }

            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return NotFound("Role not found.");
            }

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return Ok("Role deleted successfully.");
            }

            return BadRequest(result.Errors);
        }
    }
}

