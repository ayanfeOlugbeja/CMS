using AutoMapper;
using CMS.DTOs;
using CMS.Data;
using CMS.Entities;
using Microsoft.EntityFrameworkCore;
using CMS.Repositories.Interfaces;

public class BranchRepository : IBaseRepository<BranchDto, CreateBranchDto, UpdateBranchDto>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public BranchRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BranchDto> AddAsync(CreateBranchDto dto)
    {
        var branch = _mapper.Map<Branch>(dto);
        branch.IsActive = true;

        _context.Branches.Add(branch);
        await _context.SaveChangesAsync();

        return _mapper.Map<BranchDto>(branch);
    }

    public async Task<BranchDto> UpdateAsync(int id, UpdateBranchDto dto)
    {
        var branch = await _context.Branches.FindAsync(id);
        if (branch == null)
            throw new KeyNotFoundException("Branch not found");

        _mapper.Map(dto, branch);
        await _context.SaveChangesAsync();

        return _mapper.Map<BranchDto>(branch);
    }

    public async Task<bool> DeactivateAsync(int id)
    {
        var branch = await _context.Branches.FindAsync(id);
        if (branch == null || !branch.IsActive)
            return false;

        branch.IsActive = false;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<BranchDto?> GetByIdAsync(int id)
    {
        var branch = await _context.Branches
            .FirstOrDefaultAsync(b => b.Id == id); 

        return _mapper.Map<BranchDto?>(branch);
    }

    public async Task<IEnumerable<BranchDto>> GetAllAsync()
    {
        var branches = await _context.Branches.ToListAsync(); 

        return _mapper.Map<IEnumerable<BranchDto>>(branches);
    }
}
