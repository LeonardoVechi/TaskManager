using AutoMapper;
using ProjetoTaskManager.Application.DTOs.User;
using ProjetoTaskManager.Application.Interfaces;
using ProjetoTaskManager.Domain.Entities;

namespace ProjetoTaskManager.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly TokenService _tokenService;
        private readonly IEncryptService _encryptService;
        private readonly IMapper _mapper;

        public UserService(
            IUserRepository userRepository,
            TokenService tokenService,
            IEncryptService encryptService,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _encryptService = encryptService;
            _mapper = mapper;
        }

        public async Task<(bool success, string message, UserDto? data)> RegisterAsync(CreateUserDto dto)
        {
            if (await _userRepository.EmailExistsAsync(dto.Email))
                return (false, "Email já cadastrado", null);

            var user = _mapper.Map<User>(dto);
            user.Password = _encryptService.Hash(dto.Password);

            await _userRepository.AddAsync(user);
            return (true, "Usuário criado", _mapper.Map<UserDto>(user));
        }

        public async Task<(bool success, string token)> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);

            if (user == null || !_encryptService.Verify(dto.Password, user.Password))
                return (false, string.Empty);

            return (true, _tokenService.GenerateToken(user));
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return null;
            return _mapper.Map<UserDto>(user);
        }

        public async Task<(bool success, UserDto? data)> UpdateAsync(int id, UpdateUserDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return (false, null);

            _mapper.Map(dto, user);
            user.UpdateAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
            return (true, _mapper.Map<UserDto>(user));
        }

        public async Task<(bool success, string message)> UpdatePasswordAsync(int id, UpdatePasswordDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return (false, "Usuário não encontrado");

            if (!_encryptService.Verify(dto.CurrentPassword, user.Password))
                return (false, "Senha atual incorreta");

            user.Password = _encryptService.Hash(dto.NewPassword);
            user.UpdateAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
            return (true, "Senha atualizada com sucesso");
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