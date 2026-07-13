
using Microsoft.EntityFrameworkCore;
using API.Entities;
using API.Interfaces;
using API.Data;

namespace API.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly AppDbContext _context;

        public EventRepository(AppDbContext context)
        {
            _context = context;
        }

        // ============ READ ============
        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            return await _context.Events
                .Where(e => !e.IsDeleted)
                .OrderBy(e => e.StartTime)
                .ToListAsync();
        }

        public async Task<Event> GetByIdAsync(int eventId)
        {
            return await _context.Events
                .FirstOrDefaultAsync(e => e.EventId == eventId && !e.IsDeleted);
        }

        public async Task<IEnumerable<Event>> GetByDateRangeAsync(DateTime fromDate, DateTime toDate)
        {
            // Приводим даты к UTC
            var from = fromDate.ToUniversalTime().Date;
            var to = toDate.ToUniversalTime().Date.AddDays(1);

            return await _context.Events
                .Where(e => !e.IsDeleted)
                .Where(e => e.StartTime >= from && e.StartTime < to)
                .OrderBy(e => e.StartTime)
                .ToListAsync();
        }

        // ============ CREATE ============
        public async Task<Event> AddAsync(Event eventEntity)
        {
            eventEntity.IsDeleted = false;

            await _context.Events.AddAsync(eventEntity);
            await _context.SaveChangesAsync();

            return eventEntity;
        }

        // ============ DELETE ============

        public async Task<Event> SoftDeleteAsync(int eventId)
        {
            var target = await _context.Events.FindAsync(eventId);
            if (target == null)
                return null;

            target.IsDeleted = true;
            await _context.SaveChangesAsync();
            return target;
        }
    }
}