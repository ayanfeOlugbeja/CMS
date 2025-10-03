using CMS.DTOs;
using CMS.Entities;


namespace CMS.Repositories.Interfaces
{
    public interface ITownRepository
    {
        Task<IEnumerable<TownDto>> GetAllAsync();
        Task<TownDto?> GetByIdAsync(int id);
        Task<TownDto> AddAsync(CreateTownDto dto);
        Task<TownDto?> UpdateAsync(int id, UpdateTownDto dto);
        Task<bool> DeactivateAsync(int id);
    }
}
