using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoTaskManager.Application.DTOs.User
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [MaxLength(50,ErrorMessage ="Nome deve ter no máximo 50 caracteres")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha é obrigatória")]
        [MinLength(6, ErrorMessage = "Senha precisa ter no mínimo 6 caracteres")]
        public string Password { get; set; }
    }
}