using CMS.DTOs;

namespace CMS.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        Task<List<RoleDto>> GetAllRolesAsync();
        Task<RoleDto?> GetRoleByIdAsync(int id);
        Task<RoleDto> CreateRoleAsync(RoleCreateUpdateDto dto);
        Task<RoleDto?> UpdateRoleAsync(int id, RoleCreateUpdateDto dto);
        Task<bool> DeleteRoleAsync(int id);
    }
}
