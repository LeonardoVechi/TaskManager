using ProjetoTaskManager.Application.DTOs;
using ProjetoTaskManager.Application.Interfaces;
using ProjetoTaskManager.Domain.Entities;

namespace ProjetoTaskManager.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly TokenService _tokenService;

        public UserService(IUserRepository userRepository, TokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<(bool success, string message, object? data)> RegisterAsync(RegisterDto dto)
        {
            if (await _userRepository.EmailExistsAsync(dto.Email))
                return (false, "Email já cadastrado", null);

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            await _userRepository.AddAsync(user);
            return (true, "Usuário criado", new { user.Id, user.Name, user.Email });
        }

        public async Task<(bool success, string token)> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
                return (false, string.Empty);

            var token = _tokenService.GenerateToken(user);
            return (true, token);
        }

        public async Task<IEnumerable<object>> GetAllAsync() =>
            (await _userRepository.GetAllAsync())
                .Select(u => new { u.Id, u.Name, u.Email } as object);

        public async Task<object?> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return null;
            return new { user.Id, user.Name, user.Email };
        }

        public async Task<(bool success, object? data)> UpdateAsync(int id, RegisterDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return (false, null);

            user.Name = dto.Name;
            user.Email = dto.Email;
            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            await _userRepository.UpdateAsync(user);
            return (true, new { user.Id, user.Name, user.Email });
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return false;

            await _userRepository.DeleteAsync(user);
            return true;
        }
    }
}