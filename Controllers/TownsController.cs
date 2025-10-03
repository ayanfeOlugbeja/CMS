using CMS.DTOs;
using CMS.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TownsController : ControllerBase
    {
        private readonly IBaseRepository<TownDto, CreateTownDto, UpdateTownDto> _townRepository;

        public TownsController(IBaseRepository<TownDto, CreateTownDto, UpdateTownDto> townRepository)
        {
            _townRepository = townRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var towns = await _townRepository.GetAllAsync();
            return Ok(towns);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var town = await _townRepository.GetByIdAsync(id);
            if (town == null) return NotFound();
            return Ok(town);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateTownDto dto)
        {
            var newTown = await _townRepository.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = newTown.Id }, newTown);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTownDto dto)
        {
            var updatedTown = await _townRepository.UpdateAsync(id, dto);
            if (updatedTown == null) return NotFound();

            return Ok(updatedTown);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _townRepository.DeactivateAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}