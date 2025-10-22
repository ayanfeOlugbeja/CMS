using CMS.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMS.Repositories.Interfaces
{
    public interface ISubModuleRepository
    {
        Task<IEnumerable<SubModuleDto>> GetAllSubModulesAsync();
        Task<SubModuleDto?> GetSubModuleByIdAsync(int id);
        Task<SubModuleDto> CreateSubModuleAsync(SubModuleDto subModuleDto);
        Task<SubModuleDto?> UpdateSubModuleAsync(SubModuleDto subModuleDto);
        Task<bool> DeleteSubModuleAsync(int id);
        Task<IEnumerable<SubModuleDto>> GetSubModulesByModuleIdAsync(int moduleId);
    }
}
