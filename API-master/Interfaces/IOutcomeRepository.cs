using API.Entities;

namespace API.Interfaces
{
    public interface IOutcomeRepository
    {
        // ============ READ ============
        Task<IEnumerable<Outcome>> GetByEventIdAsync(Guid eventId);
        Task<Outcome> GetByIdAsync(Guid outcomeId);

        // ============ CREATE ============
        Task<Outcome> AddAsync(Outcome outcome);

        // ============ DELETE ============
        Task<Outcome> SoftDeleteAsync(Guid outcomeId);
    }
}