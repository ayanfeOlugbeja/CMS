using CMS.Data;
using CMS.DTOs;
using CMS.Entities;
using CMS.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMS.Repositories.Services
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _context;

        public RoleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<RoleDto>> GetAllRolesAsync()
        {
            return await _context.Roles
                .Include(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permission)
                .Select(r => new RoleDto
                {
                    RoleId = r.RoleId,
                    RoleName = r.RoleName,
                    Permissions = r.RolePermissions.Select(rp => new PermissionDto
                    {
                        Id = rp.Permission.PermissionId,
                        ModuleName = rp.Permission.ModuleName,
                        SubModuleName = rp.Permission.SubModuleName,
                        Description = rp.Permission.Description
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<RoleDto?> GetRoleByIdAsync(int id)
        {
            var role = await _context.Roles
                .Include(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(r => r.RoleId == id);

            if (role == null) return null;

            return new RoleDto
            {
                RoleId = role.RoleId,
                RoleName = role.RoleName,
                Permissions = role.RolePermissions.Select(rp => new PermissionDto
                {
                    Id = rp.Permission.PermissionId,
                    ModuleName = rp.Permission.ModuleName,
                    SubModuleName = rp.Permission.SubModuleName,
                    Description = rp.Permission.Description
                }).ToList()
            };
        }

        public async Task<RoleDto> CreateRoleAsync(RoleCreateUpdateDto dto)
        {
            var role = new Role
            {
                RoleName = dto.RoleName,
                RoleDescription = dto.RoleDescription
            };

            // Add permissions
            role.RolePermissions = dto.PermissionIds
                .Select(pid => new RolePermission
                {
                    PermissionId = pid,
                    Role = role
                }).ToList();

            _context.Roles.Add(role);
            await _context.SaveChangesAsync();

            // Return DTO
            return new RoleDto
            {
                RoleId = role.RoleId,
                RoleName = role.RoleName,
                Permissions = await _context.Permissions
                    .Where(p => dto.PermissionIds.Contains(p.PermissionId))
                    .Select(p => new PermissionDto
                    {
                        Id = p.PermissionId,
                        ModuleName = p.ModuleName,
                        SubModuleName = p.SubModuleName,
                        Description = p.Description
                    })
                    .ToListAsync()
            };
        }

        public async Task<RoleDto?> UpdateRoleAsync(int id, RoleCreateUpdateDto dto)
        {
            var role = await _context.Roles
                .Include(r => r.RolePermissions)
                .FirstOrDefaultAsync(r => r.RoleId == id);

            if (role == null) return null;

            // Update basic info
            role.RoleName = dto.RoleName;
            role.RoleDescription = dto.RoleDescription;

            // Update permissions
            _context.RolePermissions.RemoveRange(role.RolePermissions);
            role.RolePermissions = dto.PermissionIds
                .Select(pid => new RolePermission
                {
                    PermissionId = pid,
                    RoleId = role.RoleId
                }).ToList();

            await _context.SaveChangesAsync();

            // Return updated DTO
            return await GetRoleByIdAsync(id);
        }

        public async Task<bool> DeleteRoleAsync(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null) return false;

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
