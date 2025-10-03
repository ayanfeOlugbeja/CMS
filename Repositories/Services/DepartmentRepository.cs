using AutoMapper;
using CMS.DTOs;
using CMS.Data;
using CMS.Entities;
using Microsoft.EntityFrameworkCore;
using CMS.Repositories.Interfaces;

public class DepartmentRepository : IBaseRepository<DepartmentDto, CreateDepartmentDto, UpdateDepartmentDto>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public DepartmentRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<DepartmentDto> AddAsync(CreateDepartmentDto dto)
    {
        var department = _mapper.Map<Department>(dto);
        department.IsActive = true;

        _context.Departments.Add(department);
        await _context.SaveChangesAsync();

        return _mapper.Map<DepartmentDto>(department);
    }

    public async Task<DepartmentDto> UpdateAsync(int id, UpdateDepartmentDto dto)
    {
        var department = await _context.Departments.FindAsync(id);
        if (department == null)
            throw new KeyNotFoundException("Department not found");

        _mapper.Map(dto, department);
        await _context.SaveChangesAsync();

        return _mapper.Map<DepartmentDto>(department);
    }

    public async Task<bool> DeactivateAsync(int id)
    {
        var department = await _context.Departments.FindAsync(id);
        if (department == null || !department.IsActive)
            return false;

        department.IsActive = false;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<DepartmentDto?> GetByIdAsync(int id)
    {
        var department = await _context.Departments
            .FirstOrDefaultAsync(d => d.Id == id); 

        return _mapper.Map<DepartmentDto?>(department);
    }

    public async Task<IEnumerable<DepartmentDto>> GetAllAsync()
    {
        var departments = await _context.Departments.ToListAsync(); 

        return _mapper.Map<IEnumerable<DepartmentDto>>(departments);
    }
}