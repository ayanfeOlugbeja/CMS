using CMS.Data;
using CMS.DTOs;
using CMS.Entities;
using CMS.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMS.Repositories.Services
{
    public class ModuleRepository : IModuleRepository
    {
        private readonly AppDbContext _context;

        public ModuleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ModuleDto>> GetAllModulesAsync()
        {
            var modules = await _context.Modules
                .Include(m => m.SubModules)
                .ToListAsync();

            return modules.Select(m => new ModuleDto
            {
                ModuleId = m.ModuleId,
                ModuleName = m.ModuleName,
                Description = m.Description,
                SubModules = m.SubModules?.Select(sm => new SubModuleDto
                {
                    SubModuleId = sm.SubModuleId,
                    SubModuleName = sm.SubModuleName,
                    Description = sm.Description,
                    ModuleId = sm.ModuleId
                }).ToList()
            });
        }

        public async Task<ModuleDto?> GetModuleByIdAsync(int id)
        {
            var module = await _context.Modules
                .Include(m => m.SubModules)
                .FirstOrDefaultAsync(m => m.ModuleId == id);

            if (module == null) return null;

            return new ModuleDto
            {
                ModuleId = module.ModuleId,
                ModuleName = module.ModuleName,
                Description = module.Description,
                SubModules = module.SubModules?.Select(sm => new SubModuleDto
                {
                    SubModuleId = sm.SubModuleId,
                    SubModuleName = sm.SubModuleName,
                    Description = sm.Description,
                    ModuleId = sm.ModuleId
                }).ToList()
            };
        }

        public async Task<ModuleDto> CreateModuleAsync(ModuleDto moduleDto)
        {
            var module = new Module
            {
                ModuleName = moduleDto.ModuleName,
                Description = moduleDto.Description
            };

            _context.Modules.Add(module);
            await _context.SaveChangesAsync();

            moduleDto.ModuleId = module.ModuleId;
            return moduleDto;
        }

        public async Task<ModuleDto?> UpdateModuleAsync(ModuleDto moduleDto)
        {
            var module = await _context.Modules.FindAsync(moduleDto.ModuleId);
            if (module == null) return null;

            module.ModuleName = moduleDto.ModuleName;
            module.Description = moduleDto.Description;

            await _context.SaveChangesAsync();
            return moduleDto;
        }

        public async Task<bool> DeleteModuleAsync(int id)
        {
            var module = await _context.Modules.FindAsync(id);
            if (module == null) return false;

            _context.Modules.Remove(module);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
