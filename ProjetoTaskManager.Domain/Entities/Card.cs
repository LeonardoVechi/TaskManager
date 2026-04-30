using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoTaskManager.Domain.Entities
{
    public class Card : BaseEntity
    {
        [Required]
        [MaxLength(100, ErrorMessage = "O título pode ter no máximo 100 caracteres")]
        public string Title { get; set; }

        public string? Description { get; set; }

        public CardStatus Status { get; set; } = CardStatus.Afazer;

        public DateTime? DueDate { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

    }

    public enum CardStatus
    {
        Afazer = 0,
        EmProgresso = 1,
        Feito = 2
    }
}