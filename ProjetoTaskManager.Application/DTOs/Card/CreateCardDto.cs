using System.ComponentModel.DataAnnotations;

namespace ProjetoTaskManager.Application.DTOs
{
    public class CreateCardDto
    {
        [Required]
        [MaxLength(100, ErrorMessage = "O título deveconter no máximo 100 caracteres")]
        public string Title { get; set; }

        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }

        public int UserId { get; set; }
    }
}