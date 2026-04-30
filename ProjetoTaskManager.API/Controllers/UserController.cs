using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetoTaskManager.Application.DTOs.User;
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
        public async Task<IActionResult> Register([FromBody] CreateUserDto dto)
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
            if (!ModelState.IsValid)
                return BadRequestResponse(ModelState);

            var (success, token) = await _userService.LoginAsync(dto);
            if (!success) return UnauthorizedResponse("Email ou senha inválidos");

            return OkResponse(new { token });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return OkResponse(users);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFoundResponse("Usuário não encontrado");

            return OkResponse(user);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequestResponse(ModelState);

            var (success, data) = await _userService.UpdateAsync(id, dto);
            if (!success) return NotFoundResponse("Usuário não encontrado");

            return OkResponse(data);
        }

        [Authorize]
        [HttpPut("{id}/password")]
        public async Task<IActionResult> UpdatePassword(int id, [FromBody] UpdatePasswordDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequestResponse(ModelState);

            var (success, message) = await _userService.UpdatePasswordAsync(id, dto);
            if (!success) return BadRequestResponse(message);

            return OkResponse(message);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _userService.DeleteAsync(id);
            if (!success) return NotFoundResponse("Usuário não encontrado");

            return OkResponse("Usuário deletado com sucesso");
        }
    }
}