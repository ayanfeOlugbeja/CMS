// using Microsoft.AspNetCore.Mvc;
// using CMS.Repositories.Interfaces;
// using CMS.DTOs;

// namespace CMS.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class RolePermissionController : ControllerBase
//     {
//         private readonly IRolePermissionRepository _repo;

//         public RolePermissionController(IRolePermissionRepository repo)
//         {
//             _repo = repo;
//         }

//         //Permission
//         [HttpGet("permissions")]
//         public async Task<IActionResult> GetPermissions()
//         {
//             var result = await _repo.GetAllPermissionsAsync();
//             return Ok(new { success = true, message = "Request successful", data = result });
//         }

//         [HttpPost("permissions")]
//         public async Task<IActionResult> CreatePermission([FromBody] PermissionDto model)
//         {
//             var result = await _repo.CreatePermissionAsync(model);
//             return Ok(result);
//         }

//         [HttpPut("permissions/{id}")]
//         public async Task<IActionResult> UpdatePermission(int id, [FromBody] PermissionDto model)
//         {
//             var result = await _repo.UpdatePermissionAsync(id, model);
//             return Ok(result);
//         }

//         [HttpDelete("permissions/{id}")]
//         public async Task<IActionResult> DeletePermission(int id)
//         {
//             var result = await _repo.DeletePermissionAsync(id);
//             return Ok(result);
//         }

//         //Role
//         [HttpGet("roles")]
//         public async Task<IActionResult> GetRoles()
//         {
//             var result = await _repo.GetAllRolesAsync();
//             return Ok(new { success = true, message = "Request successful", data = result });
//         }

//         [HttpPost("roles")]
//         public async Task<IActionResult> CreateRole([FromBody] RoleDto model)
//         {
//             var result = await _repo.CreateRoleAsync(model);
//             return Ok(result);
//         }

//         [HttpPut("roles/{id}")]
//         public async Task<IActionResult> UpdateRole(int id, [FromBody] RoleDto model)
//         {
//             var result = await _repo.UpdateRoleAsync(id, model);
//             return Ok(result);
//         }

//         [HttpDelete("roles/{id}")]
//         public async Task<IActionResult> DeleteRole(int id)
//         {
//             var result = await _repo.DeleteRoleAsync(id);
//             return Ok(result);
//         }
//     }
// }
