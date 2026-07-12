
using Microsoft.EntityFrameworkCore;
using API.Entities;
using API.Data;
using API.Interfaces;

namespace API.Repositories
{
    public class MarketRepository : IMarketRepository
    {
        private readonly AppDbContext _context;

        public MarketRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Market> GetByIdWithSelectionsAsync(int marketId)
        {
            return await _context.Markets
                .Include(m => m.Selections)
                .FirstOrDefaultAsync(m => m.MarketId == marketId);
        }

        public async Task<Market> AddWithSelectionsAsync(Market market)
        {
            await _context.Markets.AddAsync(market);
            await _context.SaveChangesAsync();
            return market;
        }
    }
}