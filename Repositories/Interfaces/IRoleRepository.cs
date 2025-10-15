using CMS.DTOs;
using CMS.Entities;

namespace CMS.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetAllAsync();
        Task<Role?> GetByIdAsync(int id);
        Task<Role> CreateAsync(RoleDto dto);
        Task<Role?> UpdateAsync(int id, RoleDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
