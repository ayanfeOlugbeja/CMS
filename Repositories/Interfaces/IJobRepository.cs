using CMS.DTOs.Job;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMS.Repositories.Interfaces
{
    public interface IJobRepository
    {
        Task<IEnumerable<JobReadDto>> GetAllJobsAsync();
        Task<JobReadDto> GetJobByIdAsync(int id);
        Task<JobReadDto> AddJobAsync(JobCreateDto jobDto);
        Task<JobReadDto> UpdateJobAsync(int id, JobUpdateDto jobDto);
        Task<bool> DeleteJobAsync(int id);
    }
}
