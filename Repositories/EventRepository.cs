
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

        public async Task<IEnumerable<Event>> GetBySportIdAsync(Guid sportId)
        {
            return await _context.Events
                .Where(e => e.SportId == sportId && !e.IsDeleted)
                .OrderBy(e => e.StartTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetByStatusAsync(string status)
        {
            return await _context.Events
                .Where(e => e.Status == status && !e.IsDeleted)
                .OrderBy(e => e.StartTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetActiveAsync()
        {
            return await _context.Events
                .Where(e => !e.IsDeleted)
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

        // ============ UPDATE ============
        public async Task<Event> UpdateAsync(Event eventEntity)
        {
            var existing = await _context.Events.FindAsync(eventEntity.EventId);

            if (existing == null)
                throw new KeyNotFoundException($"Event with ID {eventEntity.EventId} not found");

            existing.SportId = eventEntity.SportId;
            existing.Team1 = eventEntity.Team1;
            existing.Team2 = eventEntity.Team2;
            existing.Score = eventEntity.Score;
            existing.Status = eventEntity.Status;
            existing.StartTime = eventEntity.StartTime;
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<Event> UpdateStatusAsync(Guid eventId, string status)
        {
            var existing = await _context.Events.FindAsync(eventId);

            if (existing == null)
                throw new KeyNotFoundException($"Event with ID {eventId} not found");

            existing.Status = status;
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<Event> UpdateScoreAsync(Guid eventId, string score)
        {
            var existing = await _context.Events.FindAsync(eventId);

            if (existing == null)
                throw new KeyNotFoundException($"Event with ID {eventId} not found");

            existing.Score = score;
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existing;
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