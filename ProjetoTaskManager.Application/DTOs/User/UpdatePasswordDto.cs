using System.ComponentModel.DataAnnotations;

namespace ProjetoTaskManager.Application.DTOs.User
{
    public class UpdatePasswordDto
    {
        [Required]
        public string CurrentPassword { get; set; }

        [Required]
        [MinLength(6, ErrorMessage ="A senha deve conter no mínimo 6 caracteres")]
        public string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword", ErrorMessage = "As senhas não conferem")]
        public string ConfirmNewPassword { get; set; }
    }
}