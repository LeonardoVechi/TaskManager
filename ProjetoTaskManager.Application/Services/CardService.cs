using AutoMapper;
using ProjetoTaskManager.Application.DTOs;
using ProjetoTaskManager.Application.DTOs.Card;
using ProjetoTaskManager.Application.Interfaces;
using ProjetoTaskManager.Domain.Entities;

namespace ProjetoTaskManager.Application.Services
{
    public class CardService
    {
        private readonly ICardRepository _cardRepository;
        private readonly IMapper _mapper;

        public CardService(ICardRepository cardRepository, IMapper mapper)
        {
            _cardRepository = cardRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CardDto>> GetAllAsync() =>
            _mapper.Map<IEnumerable<CardDto>>(await _cardRepository.GetAllAsync());

        public async Task<IEnumerable<CardDto>> GetByUserIdAsync(int userId) =>
            _mapper.Map<IEnumerable<CardDto>>(
                await _cardRepository.GetByUserIdAsync(userId));

        public async Task<CardDto?> GetByIdAsync(int id, int userId)
        {
            var card = await _cardRepository.GetByIdAndUserIdAsync(id, userId);
            if (card == null) return null;
            return _mapper.Map<CardDto>(card);
        }

        public async Task<CardDto> CreateAsync(CreateCardDto dto, int userId)
        {
            var card = _mapper.Map<Card>(dto);
            card.UserId = userId;
            await _cardRepository.AddAsync(card);
            return _mapper.Map<CardDto>(card);
        }

        public async Task<(bool success, string message, CardDto? data)> UpdateAsync(
            int id, UpdateCardDto dto, int userId)
        {
            var card = await _cardRepository.GetByIdAndUserIdAsync(id, userId);
            if (card == null) return (false, "Card não encontrado", null);

            _mapper.Map(dto, card);
            card.UpdateAt = DateTime.UtcNow;

            await _cardRepository.UpdateAsync(card);
            return (true, "Card atualizado", _mapper.Map<CardDto>(card));
        }

        public async Task<(bool success, string message, CardDto? data)> UpdateStatusAsync(
            int id, UpdateCardStatusDto dto, int userId)
        {
            var card = await _cardRepository.GetByIdAndUserIdAsync(id, userId);
            if (card == null) return (false, "Card não encontrado", null);

            card.Status = dto.Status;
            card.UpdateAt = DateTime.UtcNow;

            await _cardRepository.UpdateAsync(card);
            return (true, "Status atualizado", _mapper.Map<CardDto>(card));
        }
        public async Task<(bool success, string message)> DeleteAsync(int id, int userId)
        {
            var card = await _cardRepository.GetByIdAndUserIdAsync(id, userId);
            if (card == null) return (false, "Card não encontrado");

            await _cardRepository.DeleteAsync(card);
            return (true, "Card deletado com sucesso");
        }
    }
}