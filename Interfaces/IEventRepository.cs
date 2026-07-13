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
        Task<Event> GetByIdAsync(int eventId);
        Task<IEnumerable<Event>> GetByDateRangeAsync(DateTime fromDate, DateTime toDate);

        // ============ CREATE ============
        Task<Event> AddAsync(Event eventEntity);

        // ============ DELETE ============
        Task<Event> SoftDeleteAsync(int eventId);
    }
}