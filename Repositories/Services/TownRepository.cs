using AutoMapper;
using CMS.DTOs;
using CMS.Data;
using CMS.Entities;
using Microsoft.EntityFrameworkCore;
using CMS.Repositories.Interfaces;

public class TownRepository : IBaseRepository<TownDto, CreateTownDto, UpdateTownDto>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public TownRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TownDto> AddAsync(CreateTownDto dto)
    {
        var town = _mapper.Map<Town>(dto);
        town.IsActive = true;

        _context.Towns.Add(town);
        await _context.SaveChangesAsync();

        return _mapper.Map<TownDto>(town);
    }

    public async Task<TownDto> UpdateAsync(int id, UpdateTownDto dto)
    {
        var town = await _context.Towns.FindAsync(id);
        if (town == null)
            throw new KeyNotFoundException("Town not found");

        _mapper.Map(dto, town);
        await _context.SaveChangesAsync();

        return _mapper.Map<TownDto>(town);
    }

    public async Task<bool> DeactivateAsync(int id)
    {
        var town = await _context.Towns.FindAsync(id);
        if (town == null || !town.IsActive)
            return false;

        town.IsActive = false;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<TownDto?> GetByIdAsync(int id)
    {
        var town = await _context.Towns
            .FirstOrDefaultAsync(t => t.Id == id); 

        return _mapper.Map<TownDto?>(town);
    }

    public async Task<IEnumerable<TownDto>> GetAllAsync()
    {
        var towns = await _context.Towns.ToListAsync();

        return _mapper.Map<IEnumerable<TownDto>>(towns);
    }
}
