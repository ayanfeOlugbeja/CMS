using CMS.Data;
using CMS.DTOs.Job;
using CMS.Entities;
using CMS.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Repositories.Services
{
    public class JobRepository : IJobRepository
    {
        private readonly AppDbContext _context;

        public JobRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<JobReadDto>> GetAllJobsAsync()
        {
            return await _context.Jobs
                .Select(j => new JobReadDto
                {
                    Id = j.Id,
                    Title = j.Title,
                    Department = j.Department,
                    EmploymentType = j.EmploymentType,
                    Salary = j.Salary,
                    PostedDate = j.PostedDate,
                    ClosingDate = j.ClosingDate,
                    IsActive = j.IsActive
                }).ToListAsync();
        }

        public async Task<JobReadDto> GetJobByIdAsync(int id)
        {
            var j = await _context.Jobs.FindAsync(id);
            if (j == null) return null;

            return new JobReadDto
            {
                Id = j.Id,
                Title = j.Title,
                Department = j.Department,
                EmploymentType = j.EmploymentType,
                Salary = j.Salary,
                PostedDate = j.PostedDate,
                ClosingDate = j.ClosingDate,
                IsActive = j.IsActive
            };
        }

        public async Task<JobReadDto> AddJobAsync(JobCreateDto jobDto)
        {
            var job = new Job
            {
                Title = jobDto.Title,
                Description = jobDto.Description,
                Department = jobDto.Department,
                EmploymentType = jobDto.EmploymentType,
                Salary = jobDto.Salary,
                ClosingDate = jobDto.ClosingDate
            };

            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();

            return new JobReadDto
            {
                Id = job.Id,
                Title = job.Title,
                Department = job.Department,
                EmploymentType = job.EmploymentType,
                Salary = job.Salary,
                PostedDate = job.PostedDate,
                ClosingDate = job.ClosingDate,
                IsActive = job.IsActive
            };
        }

        public async Task<JobReadDto> UpdateJobAsync(int id, JobUpdateDto jobDto)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null) return null;

            job.Title = jobDto.Title;
            job.Description = jobDto.Description;
            job.Department = jobDto.Department;
            job.EmploymentType = jobDto.EmploymentType;
            job.Salary = jobDto.Salary;
            job.ClosingDate = jobDto.ClosingDate;
            job.IsActive = jobDto.IsActive;

            _context.Jobs.Update(job);
            await _context.SaveChangesAsync();

            return new JobReadDto
            {
                Id = job.Id,
                Title = job.Title,
                Department = job.Department,
                EmploymentType = job.EmploymentType,
                Salary = job.Salary,
                PostedDate = job.PostedDate,
                ClosingDate = job.ClosingDate,
                IsActive = job.IsActive
            };
        }

        public async Task<bool> DeleteJobAsync(int id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null) return false;

            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
