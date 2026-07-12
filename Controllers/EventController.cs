using Microsoft.AspNetCore.Mvc;
using API.DTOs;
using API.Interfaces;

namespace API.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        // GET: api/events/{id} 
        // Получить событие по id 
        [HttpGet("{id:int}")]
        public async Task<ActionResult<EventResponseDto>> GetEvent(int id)
        {
            var result = await _eventService.GetEventByIdAsync(id);

            if (result == null)
                return NotFound($"Event with ID {id} not found");

            return Ok(result);
        }

        // GET: api/events/date-range?from=date_from(for example:2024-01-01)&to=date_to(for example:2024-01-31)
        // Получить события в диапазоне дат
        [HttpGet("date-range")]
        public async Task<ActionResult<IEnumerable<EventResponseDto>>> GetEventsByDateRange(
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to)
        {
            try
            {
                // Если дата "от" не указана — берем сегодня
                var fromDate = from ?? DateTime.UtcNow.Date;

                // Если дата "до" не указана — берем через 30 дней
                var toDate = to ?? DateTime.UtcNow.Date.AddDays(30);

                var result = await _eventService.GetEventsByDateRangeAsync(fromDate, toDate);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/events
        // Добавить событие
        [HttpPost]
        public async Task<ActionResult<EventResponseDto>> CreateEvent(EventCreateDto dto)
        {
            try
            {
                var result = await _eventService.CreateEventAsync(dto);
                return CreatedAtAction(nameof(GetEvent), new { id = result.EventId }, result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PATCH: api/events/{id}/soft-delete
        // Удалить событие (мягко)
        [HttpPatch("{id:int}/soft-delete")]
        public async Task<ActionResult<EventResponseDto>> SoftDeleteEvent(int id)
        {
            try
            {
                var result = await _eventService.SoftDeleteEventAsync(id);
                if (result == null)
                    return NotFound($"Event with ID {id} not found");

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}