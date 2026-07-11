using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;

namespace API.Interfaces
{
    public interface IOutcomeService
    {
        // ============ READ ============
        Task<IEnumerable<OutcomeResponseDto>> GetOutcomesByEventIdAsync(Guid eventId);

        // ============ CREATE ============
        Task<OutcomeResponseDto> CreateOutcomeAsync(Guid eventId, OutcomeCreateDto dto);
    }
}