using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetoTaskManager.Application.DTOs;
using ProjetoTaskManager.Application.DTOs.Card;
using ProjetoTaskManager.Application.Services;

namespace ProjetoTaskManager.API.Controllers
{
    [Authorize]
    public class CardController : BaseController
    {
        private readonly CardService _cardService;

        public CardController(CardService cardService)
        {
            _cardService = cardService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = GetLoggedUserId();
            var cards = await _cardService.GetByUserIdAsync(userId);
            return OkResponse(cards);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var loggedUserId = GetLoggedUserId();
            if (loggedUserId != userId)
                return UnauthorizedResponse("Você não tem permissão para ver os cards deste usuário");

            var cards = await _cardService.GetByUserIdAsync(userId);
            return OkResponse(cards);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = GetLoggedUserId();
            var card = await _cardService.GetByIdAsync(id, userId);
            if (card == null) return NotFoundResponse("Card não encontrado");
            return OkResponse(card);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCardDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequestResponse(ModelState);

            var userId = GetLoggedUserId();
            var card = await _cardService.CreateAsync(dto, userId);
            return CreatedResponse(card);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCardDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequestResponse(ModelState);

            var userId = GetLoggedUserId();
            var (success, message, data) = await _cardService.UpdateAsync(id, dto, userId);
            if (!success) return NotFoundResponse(message);
            return OkResponse(data);
        }
        
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateCardStatusDto dto)
        {
            var userId = GetLoggedUserId();
            var (success, message, data) = await _cardService.UpdateStatusAsync(id, dto, userId);
            if (!success) return NotFoundResponse(message);
            return OkResponse(data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetLoggedUserId();
            var (success, message) = await _cardService.DeleteAsync(id, userId);
            if (!success) return NotFoundResponse(message);
            return OkResponse(message);
        }
    }
}