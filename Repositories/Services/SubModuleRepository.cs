using CMS.Data;
using CMS.DTOs;
using CMS.Entities;
using CMS.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace CMS.Repositories.Services
{
    public class SubModuleRepository : ISubModuleRepository
    {
        private readonly AppDbContext _context;

        public SubModuleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SubModuleDto>> GetAllSubModulesAsync()
        {
            var subModules = await _context.SubModules.ToListAsync();
            return subModules.Select(sm => new SubModuleDto
            {
                SubModuleId = sm.SubModuleId,
                SubModuleName = sm.SubModuleName,
                Description = sm.Description,
                ModuleId = sm.ModuleId
            });
        }

        public async Task<SubModuleDto?> GetSubModuleByIdAsync(int id)
        {
            var subModule = await _context.SubModules.FindAsync(id);
            if (subModule == null) return null;

            return new SubModuleDto
            {
                SubModuleId = subModule.SubModuleId,
                SubModuleName = subModule.SubModuleName,
                Description = subModule.Description,
                ModuleId = subModule.ModuleId
            };
        }

        public async Task<SubModuleDto> CreateSubModuleAsync(SubModuleDto subModuleDto)
        {
            var subModule = new SubModule
            {
                SubModuleName = subModuleDto.SubModuleName,
                Description = subModuleDto.Description,
                ModuleId = subModuleDto.ModuleId
            };

            _context.SubModules.Add(subModule);
            await _context.SaveChangesAsync();

            subModuleDto.SubModuleId = subModule.SubModuleId;
            return subModuleDto;
        }

        public async Task<SubModuleDto?> UpdateSubModuleAsync(SubModuleDto subModuleDto)
        {
            var subModule = await _context.SubModules.FindAsync(subModuleDto.SubModuleId);
            if (subModule == null) return null;

            subModule.SubModuleName = subModuleDto.SubModuleName;
            subModule.Description = subModuleDto.Description;
            subModule.ModuleId = subModuleDto.ModuleId;

            await _context.SaveChangesAsync();
            return subModuleDto;
        }

        public async Task<bool> DeleteSubModuleAsync(int id)
        {
            var subModule = await _context.SubModules.FindAsync(id);
            if (subModule == null) return false;

            _context.SubModules.Remove(subModule);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<SubModuleDto>> GetSubModulesByModuleIdAsync(int moduleId)
        {
            var subModules = await _context.SubModules
                .Where(sm => sm.ModuleId == moduleId)
                .ToListAsync();

            return subModules.Select(sm => new SubModuleDto
            {
                SubModuleId = sm.SubModuleId,
                SubModuleName = sm.SubModuleName,
                Description = sm.Description,
                ModuleId = sm.ModuleId
            });
        }
    }
}
