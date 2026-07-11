using API.DTOs;
using API.Entities;
using API.Interfaces;

namespace API.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _repository;

        public EventService(IEventRepository repository)
        {
            _repository = repository;
        }

        // ============ READ ============
        public async Task<IEnumerable<EventResponseDto>> GetAllEventsAsync()
        {
            var events = await _repository.GetAllAsync();
            return events.Select(MapToResponseDto);
        }

        public async Task<EventResponseDto> GetEventByIdAsync(Guid eventId)
        {
            var eventEntity = await _repository.GetByIdAsync(eventId);
            return eventEntity == null ? null : MapToResponseDto(eventEntity);
        }

        public async Task<IEnumerable<EventResponseDto>> GetEventsBySportIdAsync(Guid sportId)
        {
            var events = await _repository.GetBySportIdAsync(sportId);
            return events.Select(MapToResponseDto);
        }

        public async Task<IEnumerable<EventResponseDto>> GetEventsByStatusAsync(string status)
        {
            var events = await _repository.GetByStatusAsync(status);
            return events.Select(MapToResponseDto);
        }

        // ============ CREATE ============
        public async Task<EventResponseDto> CreateEventAsync(EventCreateDto dto)
        {
            // Проверка валидности статуса
            if (!IsValidStatus(dto.Status))
                throw new ArgumentException($"Invalid status: {dto.Status}");

            // Проверка времени
            if (dto.StartTime <= DateTime.UtcNow)
                throw new ArgumentException("Start time must be in the future");

            // Проверка: команды должны быть разными
            if (dto.Team1 == dto.Team2)
                throw new ArgumentException("Teams must be different");

            var eventEntity = new Event
            {
                EventId = Guid.NewGuid(),
                SportId = dto.SportId,
                Team1 = dto.Team1,
                Team2 = dto.Team2,
                Score = dto.Score ?? "0:0",
                Status = dto.Status ?? "scheduled",
                StartTime = dto.StartTime,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            var created = await _repository.AddAsync(eventEntity);
            return MapToResponseDto(created);
        }

        // ============ UPDATE ============
        public async Task<EventResponseDto> UpdateEventAsync(Guid eventId, EventUpdateDto dto)
        {
            var existing = await _repository.GetByIdAsync(eventId);
            if (existing == null)
                throw new KeyNotFoundException($"Event with ID {eventId} not found");

            if (!IsValidStatus(dto.Status))
                throw new ArgumentException($"Invalid status: {dto.Status}");

            existing.SportId = dto.SportId;
            existing.Team1 = dto.Team1;
            existing.Team2 = dto.Team2;
            existing.Score = dto.Score;
            existing.Status = dto.Status;
            existing.StartTime = dto.StartTime;
            existing.UpdatedAt = DateTime.UtcNow;

            var updated = await _repository.UpdateAsync(existing);
            return MapToResponseDto(updated);
        }

        public async Task<EventResponseDto> UpdateEventStatusAsync(Guid eventId, EventStatusUpdateDto dto)
        {
            var existing = await _repository.GetByIdAsync(eventId);
            if (existing == null)
                throw new KeyNotFoundException($"Event with ID {eventId} not found");

            if (!IsValidStatus(dto.Status))
                throw new ArgumentException($"Invalid status: {dto.Status}");

            var updated = await _repository.UpdateStatusAsync(eventId, dto.Status);
            return MapToResponseDto(updated);
        }

        public async Task<EventResponseDto> UpdateEventScoreAsync(Guid eventId, EventScoreUpdateDto dto)
        {
            var existing = await _repository.GetByIdAsync(eventId);
            if (existing == null)
                throw new KeyNotFoundException($"Event with ID {eventId} not found");

            var updated = await _repository.UpdateScoreAsync(eventId, dto.Score);
            return MapToResponseDto(updated);
        }

        // ============ DELETE ============

        public async Task<EventResponseDto> SoftDeleteEventAsync(Guid eventId)
        {
            var existing = await _repository.GetByIdAsync(eventId);
            if (existing == null)
                throw new KeyNotFoundException($"Event with ID {eventId} not found");

            var deleted = await _repository.SoftDeleteAsync(eventId);
            return MapToResponseDto(deleted);
        }

        // ============ HELPERS ============
        private static EventResponseDto MapToResponseDto(Event eventEntity)
        {
            return new EventResponseDto
            {
                EventId = eventEntity.EventId,
                SportId = eventEntity.SportId,
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