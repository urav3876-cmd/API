using API.Entities;

namespace API.Interfaces
{
    public interface IOutcomeRepository
    {
        // ============ READ ============
        Task<IEnumerable<Outcome>> GetByEventIdAsync(int eventId);
        Task<Outcome> GetByIdAsync(int outcomeId);

        // ============ CREATE ============
        Task<Outcome> AddAsync(Outcome outcome);

        // ============ DELETE ============
        Task<Outcome> SoftDeleteAsync(int outcomeId);
    }
}