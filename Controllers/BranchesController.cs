using CMS.DTOs;
using CMS.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BranchesController : ControllerBase
    {
        private readonly IBaseRepository<BranchDto, CreateBranchDto, UpdateBranchDto> _branchRepository;

        public BranchesController(IBaseRepository<BranchDto, CreateBranchDto, UpdateBranchDto> branchRepository)
        {
            _branchRepository = branchRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var branches = await _branchRepository.GetAllAsync();
            return Ok(branches);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var branch = await _branchRepository.GetByIdAsync(id);
            if (branch == null) return NotFound();
            return Ok(branch);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateBranchDto dto)
        {
            var newBranch = await _branchRepository.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = newBranch.Id }, newBranch);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBranchDto dto)
        {
            var updatedBranch = await _branchRepository.UpdateAsync(id, dto);
            if (updatedBranch == null) return NotFound();

            return Ok(updatedBranch);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _branchRepository.DeactivateAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}