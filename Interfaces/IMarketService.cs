using System;
using System.Threading.Tasks;
using API.DTOs;

namespace API.Interfaces
{
    public interface IMarketService
    {
        // ============ READ ============
        Task<MarketResponseDto> GetMarketByIdAsync(int marketId);

        // ============ CREATE ============
        Task<MarketResponseDto> CreateMarketAsync(int eventId, MarketCreateDto dto);
    }
}