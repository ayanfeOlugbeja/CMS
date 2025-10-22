using CMS.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CMS.Controllers
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected IActionResult HandleResult<T>(T? data, string successMessage = "Operation successful")
        {
            if (data == null)
                return NotFound(ApiResponse<object>.ErrorResponse("Resource not found"));

            return Ok(ApiResponse<T>.SuccessResponse(data, successMessage));
        }

        protected IActionResult HandleResult(string successMessage = "Operation successful")
        {
            return Ok(ApiResponse.SuccessResponse(successMessage));
        }

        protected IActionResult HandleError(string errorMessage, int statusCode = 400)
        {
            return StatusCode(statusCode, ApiResponse.ErrorResponse(errorMessage));
        }

        protected IActionResult HandleValidationErrors()
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .SelectMany(x => x.Value!.Errors)
                .Select(x => x.ErrorMessage)
                .ToList();

            return BadRequest(ApiResponse.ErrorResponse("Validation failed", errors));
        }
    }
}
