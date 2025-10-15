// using CMS.DTOs;
// using CMS.Helpers;
// using CMS.Models;
// using CMS.Responses;
// using CMS.Data;
// using CMS.Entities;
// using CMS.Repositories.Interfaces;
// using Microsoft.EntityFrameworkCore;

// namespace CMS.Repositories.Services
// {
//     public class RolePermissionRepository : IRolePermissionRepository
//     {
//         private readonly AppDbContext _context;

//         public RolePermissionRepository(AppDbContext context)
//         {
//             _context = context;
//         }

     
//         public async Task<GeneralResponse> CreatePermissionAsync(PermissionDto model)
//         {
//             var permission = new Permission
//             {
//                 ModuleName = model.ModuleName,
//                 SubModuleName = model.SubModuleName,
//                 Description = model.Description,
//                 CompanyId = model.CompanyId
//             };
//             _context.Permissions.Add(permission);
//             await _context.SaveChangesAsync();

//             return new GeneralResponse(true, "Permission created successfully.");
//         }

//         public async Task<GeneralResponse> UpdatePermissionAsync(int id, PermissionDto model)
//         {
//             var permission = await _context.Permissions.FindAsync(id);
//             if (permission == null)
//                 return new GeneralResponse(false, "Permission not found.");

//             permission.ModuleName = model.ModuleName;
//             permission.SubModuleName = model.SubModuleName;
//             permission.Description = model.Description;
//             permission.UpdatedAt = DateTime.UtcNow;
//             await _context.SaveChangesAsync();

//             return new GeneralResponse(true, "Permission updated successfully.");
//         }

//         public async Task<GeneralResponse> DeletePermissionAsync(int id)
//         {
//             var permission = await _context.Permissions.FindAsync(id);
//             if (permission == null)
//                 return new GeneralResponse(false, "Permission not found.");

//             _context.Permissions.Remove(permission);
//             await _context.SaveChangesAsync();
//             return new GeneralResponse(true, "Permission deleted successfully.");
//         }

//         public async Task<List<PermissionDto>> GetAllPermissionsAsync()
//         {
//             var permissions = await _context.Permissions.ToListAsync();
//             return permissions.Select(p => new PermissionDto
//             {
//                 PermissionId = p.PermissionId,
//                 ModuleName = p.ModuleName,
//                 SubModuleName = p.SubModuleName,
//                 Description = p.Description,
//                 CompanyId = p.CompanyId
//             }).ToList();
//         }

        
//         public async Task<GeneralResponse> CreateRoleAsync(RoleDto model)
//         {
//             var role = new Role
//             {
//                 RoleName = model.RoleName,
//                 RoleDescription = model.RoleDescription,
//                 CompanyId = model.CompanyId
//             };

//             _context.Roles.Add(role);
//             await _context.SaveChangesAsync();

//             // Assign permissions
//             // foreach (var p in model.PermissionIds)
//             // {
//             //     _context.RolePermissions.Add(new RolePermission
//             //     {
//             //         RoleId = role.RoleId,
//             //         PermissionId = p.PermissionId
//             //     });
//             // }

//             await _context.SaveChangesAsync();
//             return new GeneralResponse(true, "Role created successfully.");
//         }

//         public async Task<GeneralResponse> UpdateRoleAsync(int id, RoleDto model)
//         {
//             var role = await _context.Roles.Include(r => r.RolePermissions)
//                 .FirstOrDefaultAsync(r => r.RoleId == id);

//             if (role == null)
//                 return new GeneralResponse(false, "Role not found.");

//             role.RoleName = model.RoleName;
//             role.RoleDescription = model.RoleDescription;
//             role.UpdatedAt = DateTime.UtcNow;

//             // Update permissions
//             // _context.RolePermissions.RemoveRange(role.RolePermissions);
//             // foreach (var p in model.PermissionIds)
//             // {
//             //     _context.RolePermissions.Add(new RolePermission
//             //     {
//             //         RoleId = id,
//             //         PermissionId = p.PermissionId
//             //     });
//             // }

//             await _context.SaveChangesAsync();
//             return new GeneralResponse(true, "Role updated successfully.");
//         }

//         public async Task<GeneralResponse> DeleteRoleAsync(int id)
//         {
//             var role = await _context.Roles.FindAsync(id);
//             if (role == null)
//                 return new GeneralResponse(false, "Role not found.");

//             _context.Roles.Remove(role);
//             await _context.SaveChangesAsync();
//             return new GeneralResponse(true, "Role deleted successfully.");
//         }

//         public async Task<List<RoleDto>> GetAllRolesAsync()
//         {
//             var roles = await _context.Roles
//                 .Include(r => r.RolePermissions)
//                 .ThenInclude(rp => rp.Permission)
//                 .ToListAsync();

//             return roles.Select(r => new RoleDto
//             {
//                 RoleId = r.RoleId,
//                 RoleName = r.RoleName,
//                 CompanyId = r.CompanyId,
//                 RoleDescription = r.RoleDescription,
//                 // PermissionIds = r.RolePermissions.Select(p => new PermissionDto
//                 // {
//                 //     PermissionId = p.PermissionId,
//                 //     ModuleName = p.Permission.ModuleName,
//                 //     SubModuleName = p.Permission.SubModuleName,
//                 //     Description = p.Permission.Description,
//                 //     CompanyId = p.Permission.CompanyId
//                 // }).ToList(),
//                 EmployeeCount = r.EmployeeCount,
//                 CreatedAt = r.CreatedAt,
//                 UpdatedAt = r.UpdatedAt
//             }).ToList();
//         }
//     }
// }
