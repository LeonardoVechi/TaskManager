using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoTaskManager.Data;
using ProjetoTaskManager.Application.DTOs;
using ProjetoTaskManager.Models;
using ProjetoTaskManager.Services;

namespace ProjetoTaskManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly TokenService _tokenService;

        public UserController(AppDbContext appDbContext, TokenService tokenService)
        {
            _appDbContext = appDbContext;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var emailExistente = await _appDbContext.users
                .AnyAsync(u => u.Email == dto.Email);

            if (emailExistente)
                return Conflict("Email já cadastrado");

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _appDbContext.users.Add(user);
            await _appDbContext.SaveChangesAsync();

            return Created(string.Empty, new { user.Id, user.Name, user.Email });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _appDbContext.users
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
                return Unauthorized("Email ou senha inválidos");

            var token = _tokenService.GenerateToken(user);

            return Ok(new { token });
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _appDbContext.users
                .Select(u => new { u.Id, u.Name, u.Email })
                .ToListAsync();

            return Ok(users);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _appDbContext.users.FindAsync(id);

            if (user == null)
                return NotFound("Usuário não encontrado");

            return Ok(new { user.Id, user.Name, user.Email });
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] RegisterDto dto)
        {
            var user = await _appDbContext.users.FindAsync(id);

            if (user == null)
                return NotFound("Usuário não encontrado");

            user.Name = dto.Name;
            user.Email = dto.Email;
            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            await _appDbContext.SaveChangesAsync();

            return Ok(new { user.Id, user.Name, user.Email });
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _appDbContext.users.FindAsync(id);

            if (user == null)
                return NotFound("Usuário não encontrado");

            _appDbContext.users.Remove(user);
            await _appDbContext.SaveChangesAsync();

            return Ok("Usuário deletado com sucesso");
        }
    }
}