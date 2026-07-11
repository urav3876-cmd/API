using System;
using System.Threading.Tasks;
using API.DTOs;

namespace API.Interfaces
{
    public interface IMarketService
    {
        // ============ READ ============
        Task<MarketResponseDto> GetMarketByIdAsync(Guid marketId);

        // ============ CREATE ============
        Task<MarketResponseDto> CreateMarketAsync(Guid eventId, MarketCreateDto dto);
    }
}