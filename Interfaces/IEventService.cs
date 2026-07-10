using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;

namespace API.Interfaces
{
    public interface IEventService
    {
        // ============ READ ============
        Task<IEnumerable<EventResponseDto>> GetAllEventsAsync();
        Task<EventResponseDto> GetEventByIdAsync(Guid eventId);
        Task<IEnumerable<EventResponseDto>> GetEventsBySportIdAsync(Guid sportId);
        Task<IEnumerable<EventResponseDto>> GetEventsByStatusAsync(string status);

        // ============ CREATE ============
        Task<EventResponseDto> CreateEventAsync(EventCreateDto dto);

        // ============ UPDATE ============
        Task<EventResponseDto> UpdateEventAsync(Guid eventId, EventUpdateDto dto);
        Task<EventResponseDto> UpdateEventStatusAsync(Guid eventId, EventStatusUpdateDto dto);
        Task<EventResponseDto> UpdateEventScoreAsync(Guid eventId, EventScoreUpdateDto dto);

        // ============ DELETE ============
        Task<EventResponseDto> SoftDeleteEventAsync(Guid eventId);
    }
}