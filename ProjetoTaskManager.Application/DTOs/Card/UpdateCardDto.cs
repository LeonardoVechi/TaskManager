using System.ComponentModel.DataAnnotations;
using ProjetoTaskManager.Domain.Entities;

namespace ProjetoTaskManager.Application.DTOs
{
    public class UpdateCardDto
    {
        [Required]
        [MaxLength(100, ErrorMessage = "O título deve conter apenas 100 caracteres")]
        public string Title { get; set; }

        public string? Description { get; set; }

        public CardStatus Status { get; set; }

        public DateTime? DueDate { get; set; }
    }
}