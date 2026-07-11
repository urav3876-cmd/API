
using Microsoft.EntityFrameworkCore;
using API.Entities;
using API.Interfaces;
using API.Data;

namespace API.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly EventDbContext _context;

        public EventRepository(EventDbContext context)
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

        public async Task<Event> GetByIdAsync(Guid eventId)
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
            eventEntity.EventId = Guid.NewGuid();
            eventEntity.CreatedAt = DateTime.UtcNow;
            eventEntity.IsDeleted = false;

            await _context.Events.AddAsync(eventEntity);
            await _context.SaveChangesAsync();

            return eventEntity;
        }

        // ============ DELETE ============

        public async Task<Event> SoftDeleteAsync(Guid eventId)
        {
            var target = await _context.Events.FindAsync(eventId);
            if (target == null)
                return null;

            target.IsDeleted = true;
            target.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return target;
        }
    }
}