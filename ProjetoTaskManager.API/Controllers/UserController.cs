using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetoTaskManager.Application.DTOs;
using ProjetoTaskManager.Application.Services;

namespace ProjetoTaskManager.API.Controllers
{
    public class UserController : BaseController
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequestResponse(ModelState);

            var (success, message, data) = await _userService.RegisterAsync(dto);
            if (!success) return ConflictResponse(message);

            return CreatedResponse(data);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var (success, token) = await _userService.LoginAsync(dto);
            if (!success) return UnauthorizedResponse("Email ou senha inválidos");

            return OkResponse(new { token });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllAsync();
            return OkResponse(users);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFoundResponse("Usuário não encontrado");

            return OkResponse(user);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] RegisterDto dto)
        {
            var (success, data) = await _userService.UpdateAsync(id, dto);
            if (!success) return NotFoundResponse("Usuário não encontrado");

            return OkResponse(data);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var success = await _userService.DeleteAsync(id);
            if (!success) return NotFoundResponse("Usuário não encontrado");

            return OkResponse("Usuário deletado com sucesso");
        }
    }
}