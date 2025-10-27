using CMS.DTOs;
using CMS.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubModuleController : ControllerBase
    {
        private readonly ISubModuleRepository _subModuleRepository;

        public SubModuleController(ISubModuleRepository subModuleRepository)
        {
            _subModuleRepository = subModuleRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSubModules()
        {
            var subModules = await _subModuleRepository.GetAllSubModulesAsync();
            return Ok(subModules);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubModuleById(int id)
        {
            var subModule = await _subModuleRepository.GetSubModuleByIdAsync(id);
            if (subModule == null)
                return NotFound($"SubModule with ID {id} not found.");

            return Ok(subModule);
        }

        [HttpGet("by-module/{moduleId}")]
        public async Task<IActionResult> GetSubModulesByModuleId(int moduleId)
        {
            var subModules = await _subModuleRepository.GetSubModulesByModuleIdAsync(moduleId);
            return Ok(subModules);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubModule([FromBody] SubModuleDto subModuleDto)
        {
            if (subModuleDto == null)
                return BadRequest("Invalid submodule data.");

            var createdSubModule = await _subModuleRepository.CreateSubModuleAsync(subModuleDto);
            return CreatedAtAction(nameof(GetSubModuleById), new { id = createdSubModule.SubModuleId }, createdSubModule);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubModule(int id, [FromBody] SubModuleDto subModuleDto)
        {
            if (id != subModuleDto.SubModuleId)
                return BadRequest("SubModule ID mismatch.");

            var updatedSubModule = await _subModuleRepository.UpdateSubModuleAsync(subModuleDto);
            if (updatedSubModule == null)
                return NotFound($"SubModule with ID {id} not found.");

            return Ok(updatedSubModule);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubModule(int id)
        {
            var result = await _subModuleRepository.DeleteSubModuleAsync(id);
            if (!result)
                return NotFound($"SubModule with ID {id} not found.");

            return NoContent();
        }
    }
}
