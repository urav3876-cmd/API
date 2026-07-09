using Microsoft.EntityFrameworkCore;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;

namespace API.Services
{
    public class BetService : IBetService
    {
        private readonly BettingDbContext _context;
        private readonly IPlayerService _playerService;

        public BetService(BettingDbContext context, IPlayerService playerService)
        {
            _context = context;
            _playerService = playerService;
        }

        public async Task<BetResponseDto> CreateBetAsync(BetCreateDto dto)
        {

            var playerExists = await _playerService.PlayerExistsAsync(dto.PlayerId);
            if (!playerExists)
                throw new KeyNotFoundException($"Player with ID {dto.PlayerId} not found");

            if (dto.Selections == null || !dto.Selections.Any())
                throw new ArgumentException("At least one selection is required");

            var bet = new Bet
            {
                PlayerId = dto.PlayerId,
                Amount = dto.Amount,
                CalculatedPayout = dto.CalculatedPayout,
                ActualPayout = dto.ActualPayout,
                Status = "pending",
                CreatedAt = DateTime.UtcNow,
                Selections = dto.Selections.Select(s => new BetSelection
                {
                    SelectionId = s.SelectionId,
                    MarketId = s.MarketId,
                    SelectionName = s.SelectionName,
                    Coefficient = s.Coefficient,
                    IsWon = null  // При создании исход неизвестен
                }).ToList()
            };

            _context.Bets.Add(bet);
            await _context.SaveChangesAsync();

            //проверка правильности сохранения и получение созданной ставки с включенными селекциями
            var createdBet = await _context.Bets
                .Include(b => b.Selections)
                .FirstAsync(b => b.BetId == bet.BetId);

            return MapToResponse(createdBet);
        }

        public async Task<BetResponseDto?> GetBetAsync(int id)
        {
            var bet = await _context.Bets
                .Include(b => b.Selections)
                .FirstOrDefaultAsync(b => b.BetId == id);

            if (bet == null)
                return null;

            return MapToResponse(bet);
        }

        private static BetResponseDto MapToResponse(Bet bet)
        {
            return new BetResponseDto
            {
                BetId = bet.BetId,
                PlayerId = bet.PlayerId,
                Amount = bet.Amount,
                CalculatedPayout = bet.CalculatedPayout,
                ActualPayout = bet.ActualPayout,
                Status = bet.Status,
                CreatedAt = bet.CreatedAt,
                SettledAt = bet.SettledAt,
                Selections = bet.Selections.Select(s => new BetSelectionDto
                {
                    SelectionId = s.SelectionId,
                    MarketId = s.MarketId,
                    SelectionName = s.SelectionName,
                    Coefficient = s.Coefficient,
                    IsWon = s.IsWon
                }).ToList()
            };
        }
    }
}