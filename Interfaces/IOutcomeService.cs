using API.DTOs;

namespace API.Interfaces
{
    public interface IOutcomeService
    {
        // ============ READ ============
        Task<IEnumerable<OutcomeResponseDto>> GetOutcomesByEventIdAsync(int eventId);

        // ============ CREATE ============
        Task<OutcomeResponseDto> CreateOutcomeAsync(int eventId, OutcomeCreateDto dto);
    }
}