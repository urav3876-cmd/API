using API.DTOs;
using API.Entities;
using API.Interfaces;

namespace API.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IOutcomeRepository _outcomeRepository; 

        public EventService(IEventRepository eventRepository, IOutcomeRepository outcomeRepository)
        {
            _eventRepository = eventRepository;
            _outcomeRepository = outcomeRepository; 
        }

        // ============ READ ============
        public async Task<IEnumerable<EventResponseDto>> GetAllEventsAsync()
        {
            var events = await _eventRepository.GetAllAsync();
            return events.Select(MapToResponseDto);
        }

        public async Task<EventResponseDto> GetEventByIdAsync(Guid eventId)
        {
            var eventEntity = await _eventRepository.GetByIdAsync(eventId);
            return eventEntity == null ? null : MapToResponseDto(eventEntity);
        }

        public async Task<IEnumerable<EventResponseDto>> GetEventsByDateRangeAsync(DateTime fromDate, DateTime toDate)
        {
            var from = fromDate.ToUniversalTime().Date;
            var to = toDate.ToUniversalTime().Date;

            if (to < from)
                throw new ArgumentException("End date cannot be earlier than start date");

            if (fromDate == DateTime.MinValue)
                from = DateTime.UtcNow.Date;

            if (toDate == DateTime.MinValue)
                to = DateTime.UtcNow.Date.AddDays(30);

            var events = await _eventRepository.GetByDateRangeAsync(from, to);
            return events.Select(MapToResponseDto);
        }

        // ============ CREATE ============
        public async Task<EventResponseDto> CreateEventAsync(EventCreateDto dto)
        {
            if (!IsValidStatus(dto.Status))
                throw new ArgumentException($"Invalid status: {dto.Status}");

            if (dto.StartTime <= DateTime.UtcNow)
                throw new ArgumentException("Start time must be in the future");

            if (dto.Team1 == dto.Team2)
                throw new ArgumentException("Teams must be different");

            var eventEntity = new Event
            {
                EventId = Guid.NewGuid(),
                SportName = dto.SportName,
                Team1 = dto.Team1,
                Team2 = dto.Team2,
                Score = dto.Score ?? "0:0",
                Status = dto.Status ?? "scheduled",
                StartTime = dto.StartTime,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            var created = await _eventRepository.AddAsync(eventEntity);
            return MapToResponseDto(created);
        }

        // ============ DELETE ============
        public async Task<EventResponseDto> SoftDeleteEventAsync(Guid eventId)
        {
            var existing = await _eventRepository.GetByIdAsync(eventId);
            if (existing == null)
                throw new KeyNotFoundException($"Event with ID {eventId} not found");

            // удаляем событие
            var deleted = await _eventRepository.SoftDeleteAsync(eventId);

            // удаляем все связанные Outcomes
            var outcomes = await _outcomeRepository.GetByEventIdAsync(eventId);
            foreach (var outcome in outcomes)
            {
                await _outcomeRepository.SoftDeleteAsync(outcome.OutcomeId);
            }

            return MapToResponseDto(deleted);
        }

        // ============ HELPERS ============
        private static EventResponseDto MapToResponseDto(Event eventEntity)
        {
            return new EventResponseDto
            {
                EventId = eventEntity.EventId,
                SportName = eventEntity.SportName,
                Team1 = eventEntity.Team1,
                Team2 = eventEntity.Team2,
                Score = eventEntity.Score,
                Status = eventEntity.Status,
                StartTime = eventEntity.StartTime,
                CreatedAt = eventEntity.CreatedAt,
                UpdatedAt = eventEntity.UpdatedAt
            };
        }

        private static bool IsValidStatus(string status)
        {
            var validStatuses = new[] { "scheduled", "inprogress", "finished", "postponed", "cancelled" };
            return validStatuses.Contains(status.ToLower());
        }
    }
}