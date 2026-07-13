using API.DTOs;

namespace API.Interfaces
{
    public interface IEventService
    {
        // ============ READ ============
        Task<IEnumerable<EventResponseDto>> GetAllEventsAsync();
        Task<EventResponseDto> GetEventByIdAsync(int eventId);
        Task<IEnumerable<EventResponseDto>> GetEventsByDateRangeAsync(DateTime fromDate, DateTime toDate);

        // ============ CREATE ============
        Task<EventResponseDto> CreateEventAsync(EventCreateDto dto);

        // ============ DELETE ============
        Task<EventResponseDto> SoftDeleteEventAsync(int eventId);
    }
}