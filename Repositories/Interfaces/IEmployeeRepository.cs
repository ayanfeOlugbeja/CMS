
using CMS.DTOs;
using CMS.Entities;

namespace CMS.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<EmployeeDto>> GetAllAsync();
        Task<EmployeeDto?> GetByIdAsync(int id);
        Task<EmployeeDto> AddAsync(CreateEmployeeDto dto);
        Task<EmployeeDto?> UpdateEmployee(int id, UpdateEmployeeDto dto);
        Task<bool> DeactivateAsync(int id);
    }
}
