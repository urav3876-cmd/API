using System;
using System.Threading.Tasks;
using API.Entities;

namespace API.Interfaces
{
    public interface IMarketRepository
    {
        // ============ READ ============
        Task<Market> GetByIdWithSelectionsAsync(Guid marketId);

        // ============ CREATE ============
        Task<Market> AddWithSelectionsAsync(Market market);
    }
}