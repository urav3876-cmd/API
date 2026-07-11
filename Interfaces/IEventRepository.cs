using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;

namespace API.Interfaces
{
    public interface IEventRepository
    {
        // ============ READ ============
        Task<IEnumerable<Event>> GetAllAsync();
        Task<Event> GetByIdAsync(Guid eventId);
        Task<IEnumerable<Event>> GetBySportIdAsync(Guid sportId);
        Task<IEnumerable<Event>> GetByStatusAsync(string status);
        Task<IEnumerable<Event>> GetActiveAsync();

        // ============ CREATE ============
        Task<Event> AddAsync(Event eventEntity);

        // ============ UPDATE ============
        Task<Event> UpdateAsync(Event eventEntity);
        Task<Event> UpdateStatusAsync(Guid eventId, string status);
        Task<Event> UpdateScoreAsync(Guid eventId, string score);

        // ============ DELETE ============
        Task<Event> SoftDeleteAsync(Guid eventId);
    }
}