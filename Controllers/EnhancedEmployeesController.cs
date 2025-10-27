using CMS.DTOs;
using CMS.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnhancedEmployeesController : BaseController
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EnhancedEmployeesController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        /// <summary>
        /// Get all employees with optional filtering
        /// </summary>
        /// <param name="isActive">Filter by active status</param>
        /// <returns>List of employees</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<EmployeeDto>>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 401)]
        [ProducesResponseType(typeof(ApiResponse), 403)]
        public async Task<IActionResult> GetAll([FromQuery] bool? isActive)
        {
            if (!ModelState.IsValid)
                return HandleValidationErrors();

            var employees = await _employeeRepository.GetAllAsync();

            if (isActive.HasValue)
                employees = employees.Where(e => e.IsActive == isActive.Value).ToList();

            return HandleResult(employees, "Employees retrieved successfully");
        }

        /// <summary>
        /// Get employee by ID
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <returns>Employee details</returns>
        [Authorize(Roles = "Admin,Employee")]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<EmployeeDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 401)]
        [ProducesResponseType(typeof(ApiResponse), 403)]
        public async Task<IActionResult> GetById(int id)
        {
            if (!ModelState.IsValid)
                return HandleValidationErrors();

            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null) 
                return HandleError("Employee not found", 404);

            // Role-based access control
            if (User.IsInRole("Employee"))
            {
                var email = User.Identity?.Name;
                if (employee.Email != email) 
                    return HandleError("Access denied", 403);
                if (!employee.IsActive) 
                    return HandleError("Employee account is inactive", 403);
            }

            return HandleResult(employee, "Employee retrieved successfully");
        }

        /// <summary>
        /// Create a new employee
        /// </summary>
        /// <param name="dto">Employee creation data</param>
        /// <returns>Created employee</returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<EmployeeDto>), 201)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 401)]
        [ProducesResponseType(typeof(ApiResponse), 403)]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeDto dto)
        {
            if (!ModelState.IsValid)
                return HandleValidationErrors();

            try
            {
                var newEmployee = await _employeeRepository.AddAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = newEmployee.Id }, 
                    ApiResponse<EmployeeDto>.SuccessResponse(newEmployee, "Employee created successfully"));
            }
            catch (Exception ex)
            {
                return HandleError($"Failed to create employee: {ex.Message}", 400);
            }
        }


        /// Update an existing employee
       
        /// <param name="id">Employee ID</param>
        /// <param name="dto">Employee update data</param>
        /// <returns>Updated employee</returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<EmployeeDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 401)]
        [ProducesResponseType(typeof(ApiResponse), 403)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEmployeeDto dto)
        {
            if (!ModelState.IsValid)
                return HandleValidationErrors();

            try
            {
                await _employeeRepository.UpdateEmployee(id, dto);
                var updatedEmployee = await _employeeRepository.GetByIdAsync(id);
                
                if (updatedEmployee == null) 
                    return HandleError("Employee not found", 404);

                return HandleResult(updatedEmployee, "Employee updated successfully");
            }
            catch (Exception ex)
            {
                return HandleError($"Failed to update employee: {ex.Message}", 400);
            }
        }

        /// <summary>
        /// Deactivate an employee
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <returns>Success message</returns>
        [Authorize(Roles = "Admin")]
        [HttpPatch("{id}/deactivate")]
        [ProducesResponseType(typeof(ApiResponse), 204)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 401)]
        [ProducesResponseType(typeof(ApiResponse), 403)]
        public async Task<IActionResult> Deactivate(int id)
        {
            if (!ModelState.IsValid)
                return HandleValidationErrors();

            try
            {
                var success = await _employeeRepository.DeactivateAsync(id);
                if (!success) 
                    return HandleError("Employee not found", 404);

                return HandleResult("Employee deactivated successfully");
            }
            catch (InvalidOperationException ex)
            {
                return HandleError(ex.Message, 400);
            }
            catch (Exception ex)
            {
                return HandleError($"Failed to deactivate employee: {ex.Message}", 400);
            }
        }
    }
}
