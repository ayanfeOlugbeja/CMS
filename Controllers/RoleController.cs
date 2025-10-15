using CMS.DTOs;
using CMS.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository _repository;

        public RoleController(IRoleRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _repository.GetAllAsync();
            return Ok(new { success = true, message = "Request successful", data = roles });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var role = await _repository.GetByIdAsync(id);
            if (role == null)
                return NotFound(new { success = false, message = "Role not found" });

            return Ok(new { success = true, message = "Request successful", data = role });
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleDto dto)
        {
            var role = await _repository.CreateAsync(dto);
            return Ok(new { success = true, message = "Role created successfully", data = role });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, RoleDto dto)
        {
            var updatedRole = await _repository.UpdateAsync(id, dto);
            if (updatedRole == null)
                return NotFound(new { success = false, message = "Role not found" });

            return Ok(new { success = true, message = "Role updated successfully", data = updatedRole });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _repository.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { success = false, message = "Role not found" });

            return Ok(new { success = true, message = "Role deleted successfully" });
        }
    }
}
