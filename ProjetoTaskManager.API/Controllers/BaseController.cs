using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace ProjetoTaskManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController : ControllerBase
    {

         protected int GetLoggedUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            return int.Parse(claim!.Value);
        }
        protected IActionResult OkResponse(object? data = null) =>
            Ok(new { success = true, data });

        protected IActionResult CreatedResponse(object? data = null) =>
            StatusCode(201, new { success = true, data });

        protected IActionResult NotFoundResponse(string message) =>
            NotFound(new { success = false, message });

        protected IActionResult BadRequestResponse(object errors) =>
            BadRequest(new { success = false, errors });

        protected IActionResult ConflictResponse(string message) =>
            Conflict(new { success = false, message });

        protected IActionResult UnauthorizedResponse(string message) =>
            Unauthorized(new { success = false, message });
    }
}