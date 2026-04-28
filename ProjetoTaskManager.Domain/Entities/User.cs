using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoTaskManager.Domain.Entities
{
    public class User : BaseEntity
    {

        [Required(ErrorMessage = "Nome é um campo obrigatório")]
        [MaxLength(50, ErrorMessage ="Nome pode conter no máximo 50 caracteres")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email é um campo obrigatório")]
        [EmailAddress(ErrorMessage ="Email inválido")]
        public string Email { get; set; }

    
        [Required(ErrorMessage = "Senha é um campo obrigatório")]
        public string Password { get; set; }
    }
}