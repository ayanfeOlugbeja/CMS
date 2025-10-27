using CMS.DTOs;
using CMS.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModuleController : ControllerBase
    {
        private readonly IModuleRepository _moduleRepository;

        public ModuleController(IModuleRepository moduleRepository)
        {
            _moduleRepository = moduleRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllModules()
        {
            var modules = await _moduleRepository.GetAllModulesAsync();
            return Ok(modules);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetModuleById(int id)
        {
            var module = await _moduleRepository.GetModuleByIdAsync(id);
            if (module == null)
                return NotFound($"Module with ID {id} not found.");
            return Ok(module);
        }

        [HttpPost]
        public async Task<IActionResult> CreateModule([FromBody] ModuleDto moduleDto)
        {
            if (moduleDto == null)
                return BadRequest("Invalid module data.");

            var createdModule = await _moduleRepository.CreateModuleAsync(moduleDto);
            return CreatedAtAction(nameof(GetModuleById), new { id = createdModule.ModuleId }, createdModule);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateModule(int id, [FromBody] ModuleDto moduleDto)
        {
            if (id != moduleDto.ModuleId)
                return BadRequest("Module ID mismatch.");

            var updatedModule = await _moduleRepository.UpdateModuleAsync(moduleDto);
            if (updatedModule == null)
                return NotFound($"Module with ID {id} not found.");

            return Ok(updatedModule);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModule(int id)
        {
            var result = await _moduleRepository.DeleteModuleAsync(id);
            if (!result)
                return NotFound($"Module with ID {id} not found.");

            return NoContent();
        }
    }
}
