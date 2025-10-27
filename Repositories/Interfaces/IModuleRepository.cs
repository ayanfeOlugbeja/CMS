using CMS.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMS.Repositories.Interfaces
{
    public interface IModuleRepository
    {
        Task<IEnumerable<ModuleDto>> GetAllModulesAsync();
        Task<ModuleDto?> GetModuleByIdAsync(int id);
        Task<ModuleDto> CreateModuleAsync(ModuleDto moduleDto);
        Task<ModuleDto?> UpdateModuleAsync(ModuleDto moduleDto);
        Task<bool> DeleteModuleAsync(int id);
    }
}
