using API.DTOs;
using API.Entities;
using API.Interfaces;

namespace API.Services
{
    public class OutcomeService : IOutcomeService
    {
        private readonly IOutcomeRepository _outcomeRepository;
        private readonly IEventRepository _eventRepository;

        public OutcomeService(IOutcomeRepository outcomeRepository, IEventRepository eventRepository)
        {
            _outcomeRepository = outcomeRepository;
            _eventRepository = eventRepository;
        }

        // ============ READ ============
        public async Task<IEnumerable<OutcomeResponseDto>> GetOutcomesByEventIdAsync(int eventId)
        {
            var eventExists = await _eventRepository.GetByIdAsync(eventId);
            if (eventExists == null)
                throw new KeyNotFoundException($"Event with ID {eventId} not found");

            var outcomes = await _outcomeRepository.GetByEventIdAsync(eventId);
            return outcomes.Select(MapToResponseDto);
        }

        // ============ CREATE ============
        public async Task<OutcomeResponseDto> CreateOutcomeAsync(int eventId, OutcomeCreateDto dto)
        {
            var eventExists = await _eventRepository.GetByIdAsync(eventId);
            if (eventExists == null)
                throw new KeyNotFoundException($"Event with ID {eventId} not found");

            if (string.IsNullOrWhiteSpace(dto.OutcomeName))
                throw new ArgumentException("Outcome name is required");

            if (string.IsNullOrWhiteSpace(dto.OutcomeScore))
                throw new ArgumentException("Outcome score is required");

            var outcome = new Outcome
            {
                EventId = eventId,
                OutcomeName = dto.OutcomeName.Trim(),
                OutcomeScore = dto.OutcomeScore.Trim(),
                IsDeleted = false
            };

            var created = await _outcomeRepository.AddAsync(outcome);
            return MapToResponseDto(created);
        }

        // ============ HELPERS ============
        private static OutcomeResponseDto MapToResponseDto(Outcome outcome)
        {
            return new OutcomeResponseDto
            {
                OutcomeId = outcome.OutcomeId,
                EventId = outcome.EventId,
                OutcomeName = outcome.OutcomeName,
                OutcomeScore = outcome.OutcomeScore
            };
        }
    }
}