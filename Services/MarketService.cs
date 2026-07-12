
using API.DTOs;
using API.Entities;
using API.Interfaces;

namespace API.Services
{
    public class MarketService : IMarketService
    {
        private readonly IMarketRepository _marketRepository;
        private readonly IEventRepository _eventRepository;

        public MarketService(IMarketRepository marketRepository, IEventRepository eventRepository)
        {
            _marketRepository = marketRepository;
            _eventRepository = eventRepository;
        }

        public async Task<MarketResponseDto> GetMarketByIdAsync(int marketId)
        {
            var market = await _marketRepository.GetByIdWithSelectionsAsync(marketId);
            if (market == null)
                throw new KeyNotFoundException($"Market with ID {marketId} not found");

            return MapToResponseDto(market);
        }

        public async Task<MarketResponseDto> CreateMarketAsync(int eventId, MarketCreateDto dto)
        {
            var eventExists = await _eventRepository.GetByIdAsync(eventId);
            if (eventExists == null)
                throw new KeyNotFoundException($"Event with ID {eventId} not found");

            if (dto.Selections == null || dto.Selections.Count == 0)
                throw new ArgumentException("Market must have at least one selection");

            var market = new Market
            {
                EventId = eventId,
                MarketType = dto.MarketType.Trim(),
                MarketValue = dto.MarketValue.Trim(),
                IsAvailable = dto.IsAvailable,
                Selections = dto.Selections.Select(s => new Selection
                {
                    SelectionName = s.SelectionName.Trim(),
                    Coefficient = s.Coefficient,
                    IsWon = s.IsWon
                }).ToList()
            };

            var created = await _marketRepository.AddWithSelectionsAsync(market);
            return MapToResponseDto(created);
        }

        private static MarketResponseDto MapToResponseDto(Market market)
        {
            return new MarketResponseDto
            {
                MarketId = market.MarketId,
                EventId = market.EventId,
                MarketType = market.MarketType,
                MarketValue = market.MarketValue,
                IsAvailable = market.IsAvailable,
                Selections = market.Selections.Select(s => new SelectionResponseDto
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