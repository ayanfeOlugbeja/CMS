using CMS.DTOs;
using CMS.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleRepository _repo;

        public RolesController(IRoleRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _repo.GetAllRolesAsync();
            return Ok(new { success = true, message = "Roles fetched successfully", data = roles });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var role = await _repo.GetRoleByIdAsync(id);
            if (role == null)
                return NotFound(new { success = false, message = "Role not found" });

            return Ok(new { success = true, message = "Role fetched successfully", data = role });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoleCreateUpdateDto dto)
        {
            var role = await _repo.CreateRoleAsync(dto);
            return Ok(new { success = true, message = "Role created successfully", data = role });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] RoleCreateUpdateDto dto)
        {
            var updated = await _repo.UpdateRoleAsync(id, dto);
            if (updated == null)
                return NotFound(new { success = false, message = "Role not found" });

            return Ok(new { success = true, message = "Role updated successfully", data = updated });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _repo.DeleteRoleAsync(id);
            if (!deleted)
                return NotFound(new { success = false, message = "Role not found" });

            return Ok(new { success = true, message = "Role deleted successfully" });
        }
    }
}
