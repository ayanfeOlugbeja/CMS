using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CMS.DTOs;
using CMS.Repositories.Interfaces;
using CMS.Entities;

namespace CMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionRepository _repo;

        public PermissionController(IPermissionRepository repo)
        {
            _repo = repo;
        }

        // Get all permissions
        [HttpGet("permissions")]
        public async Task<IActionResult> GetPermissions()
        {
            var result = await _repo.GetAllPermissionsAsync();
            return Ok(new { success = true, message = "Request successful", data = result });
        }

        // Create new permission
        [HttpPost("permissions")]
        public async Task<IActionResult> CreatePermission([FromBody] Permission model)
        {
            if (model == null)
                return BadRequest(new { success = false, message = "Invalid request body" });

            var result = await _repo.CreatePermissionAsync(model);
            return Ok(new { success = true, message = "Permission created successfully", data = result });
        }

        // Update existing permission
        [HttpPut("permissions/{id}")]
        public async Task<IActionResult> UpdatePermission(int id, [FromBody] Permission model)
        {
            if (model == null)
                return BadRequest(new { success = false, message = "Invalid request body" });

            var result = await _repo.UpdatePermissionAsync(id, model);
            if (result == null)
                return NotFound(new { success = false, message = "Permission not found" });

            return Ok(new { success = true, message = "Permission updated successfully", data = result });
        }

        // Delete permission
        [HttpDelete("permissions/{id}")]
        public async Task<IActionResult> DeletePermission(int id)
        {
            var result = await _repo.DeletePermissionAsync(id);

            if (!result)
                return NotFound(new { success = false, message = "Permission not found or already deleted" });

            return Ok(new { success = true, message = "Permission deleted successfully" });
        }
    }
}
