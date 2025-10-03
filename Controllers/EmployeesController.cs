using CMS.DTOs;
using CMS.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeesController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }


        // For Admin

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] bool? isActive)
        {
            var employees = await _employeeRepository.GetAllAsync();

            if (isActive.HasValue)
                employees = employees.Where(e => e.IsActive == isActive.Value).ToList();

            return Ok(employees);
        }


        // Both Admin & Employee 
        [Authorize(Roles = "Admin,Employee")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null) return NotFound();

            if (User.IsInRole("Employee"))
            {
                var email = User.Identity?.Name;
                if (employee.Email != email) return Forbid();
                if (!employee.IsActive) return Forbid();
            }
            return Ok(employee);

        }

        // Admin-only actions
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeDto dto)
        {
            var newEmployee = await _employeeRepository.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = newEmployee.Id }, newEmployee);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEmployeeDto dto)
        {
            await _employeeRepository.UpdateEmployee(id, dto);
            var updatedEmployee = await _employeeRepository.GetByIdAsync(id);
            if (updatedEmployee == null) return NotFound();

            return Ok(updatedEmployee);
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{id}/deactivate")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Deactivate(int id)
        {
            try
            {
                var success = await _employeeRepository.DeactivateAsync(id);
                if (!success) return NotFound();
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
