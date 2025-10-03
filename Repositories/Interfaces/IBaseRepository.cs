using CMS.DTOs;

namespace CMS.Repositories.Interfaces
{
    public interface IBaseRepository<TDto, TCreateDto, TUpdateDto>
    {
        Task<TDto> AddAsync(TCreateDto dto);
        Task<TDto> UpdateAsync(int id, TUpdateDto dto);
        Task<bool> DeactivateAsync(int id);
        Task<TDto?> GetByIdAsync(int id);
        Task<IEnumerable<TDto>> GetAllAsync();
        //Task UpdateEmployee(int id, UpdateEmployeeDto dto);
    }
}
