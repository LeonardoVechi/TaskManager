using System.ComponentModel.DataAnnotations;

namespace ProjetoTaskManager.Application.DTOs.User
{
    public class UpdateUserDto
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Nome pode conter no máximo 50 caracteres")]
        public string Name { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; }
    }
}