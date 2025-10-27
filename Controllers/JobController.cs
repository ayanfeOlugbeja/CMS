using CMS.DTOs.Job;
using CMS.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly IJobRepository _repo;

        public JobsController(IJobRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllJobs()
        {
            var jobs = await _repo.GetAllJobsAsync();
            return Ok(new { success = true, data = jobs });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetJobById(int id)
        {
            var job = await _repo.GetJobByIdAsync(id);
            if (job == null)
                return NotFound(new { success = false, message = "Job not found" });

            return Ok(new { success = true, data = job });
        }

        [HttpPost]
        public async Task<IActionResult> AddJob(JobCreateDto dto)
        {
            var added = await _repo.AddJobAsync(dto);
            return Ok(new { success = true, data = added });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJob(int id, JobUpdateDto dto)
        {
            var updated = await _repo.UpdateJobAsync(id, dto);
            if (updated == null)
                return NotFound(new { success = false, message = "Job not found" });

            return Ok(new { success = true, data = updated });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJob(int id)
        {
            var deleted = await _repo.DeleteJobAsync(id);
            if (!deleted)
                return NotFound(new { success = false, message = "Job not found or already deleted" });

            return Ok(new { success = true, message = "Job deleted successfully" });
        }
    }
}
