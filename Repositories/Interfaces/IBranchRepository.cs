using CMS.DTOs;
using CMS.Entities;


namespace CMS.Repositories.Interfaces

{
    public interface IBranchRepository
    {
        Task<IEnumerable<BranchDto>> GetAllAsync();
        Task<BranchDto?> GetByIdAsync(int id);
        Task<BranchDto> AddAsync(CreateBranchDto dto);
        Task<BranchDto?> UpdateAsync(int id, UpdateBranchDto dto);
        Task<bool> DeactivateAsync(int id);
    }
}
