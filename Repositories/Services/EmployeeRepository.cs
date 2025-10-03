using AutoMapper;
using CMS.Data;
using CMS.DTOs;
using CMS.Entities;
using CMS.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMS.Repositories.Services
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
        {
            var employees = await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Branch)
                .Include(e => e.Town)
                .ToListAsync();

            return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        }

        public async Task<EmployeeDto?> GetByIdAsync(int id)
        {
            var employee = await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Branch)
                .Include(e => e.Town)
                .FirstOrDefaultAsync(e => e.Id == id);

            return employee == null ? null : _mapper.Map<EmployeeDto>(employee);
        }

        public async Task<EmployeeDto> AddAsync(CreateEmployeeDto dto)
        {
            if (!await _context.Departments.AnyAsync(d => d.Id == dto.DepartmentId))
                throw new ArgumentException($"Department {dto.DepartmentId} not found");

            if (!await _context.Branches.AnyAsync(b => b.Id == dto.BranchId))
                throw new ArgumentException($"Branch {dto.BranchId} not found");

            if (!await _context.Towns.AnyAsync(t => t.Id == dto.TownId))
                throw new ArgumentException($"Town {dto.TownId} not found");

            var employee = _mapper.Map<Employee>(dto);
            employee.IsActive = true;

            await base.AddAsync(employee);

            var created = await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Branch)
                .Include(e => e.Town)
                .FirstOrDefaultAsync(e => e.Id == employee.Id);

            return _mapper.Map<EmployeeDto>(created!);
        }

        public async Task<EmployeeDto> UpdateEmployee(int id, UpdateEmployeeDto dto)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
                throw new ArgumentException($"Employee {id} not found");

            if (dto.DepartmentId.HasValue &&
                !await _context.Departments.AnyAsync(d => d.Id == dto.DepartmentId.Value))
                throw new ArgumentException($"Department {dto.DepartmentId} not found");

            if (dto.BranchId.HasValue &&
                !await _context.Branches.AnyAsync(b => b.Id == dto.BranchId.Value))
                throw new ArgumentException($"Branch {dto.BranchId} not found");

            if (dto.TownId.HasValue &&
                !await _context.Towns.AnyAsync(t => t.Id == dto.TownId.Value))
                throw new ArgumentException($"Town {dto.TownId} not found");

            _mapper.Map(dto, employee);
            await _context.SaveChangesAsync();

            var updated = await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Branch)
                .Include(e => e.Town)
                .FirstOrDefaultAsync(e => e.Id == id);

            return _mapper.Map<EmployeeDto>(updated!);
        }

        public async Task<bool> DeactivateAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null || !employee.IsActive)
                return false;

            employee.IsActive = false;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
