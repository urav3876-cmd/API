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

        // GET: api/events
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventResponseDto>>> GetAllEvents()
        {
            var result = await _eventService.GetAllEventsAsync();
            return Ok(result);
        }

        // GET: api/events/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EventResponseDto>> GetEvent(Guid id)
        {
            var result = await _eventService.GetEventByIdAsync(id);

            if (result == null)
                return NotFound($"Event with ID {id} not found");

            return Ok(result);
        }

        // GET: api/events/sport/{sportId}
        [HttpGet("sport/{sportId:guid}")]
        public async Task<ActionResult<IEnumerable<EventResponseDto>>> GetEventsBySport(Guid sportId)
        {
            var result = await _eventService.GetEventsBySportIdAsync(sportId);
            return Ok(result);
        }

        // GET: api/events/status/{status}
        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<EventResponseDto>>> GetEventsByStatus(string status)
        {
            var result = await _eventService.GetEventsByStatusAsync(status);
            return Ok(result);
        }

        // POST: api/events
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

        // PUT: api/events/{id}
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<EventResponseDto>> UpdateEvent(Guid id, EventUpdateDto dto)
        {
            try
            {
                var result = await _eventService.UpdateEventAsync(id, dto);
                return Ok(result);
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

        // PATCH: api/events/{id}/status
        [HttpPatch("{id:guid}/status")]
        public async Task<ActionResult<EventResponseDto>> UpdateEventStatus(Guid id, EventStatusUpdateDto dto)
        {
            try
            {
                var result = await _eventService.UpdateEventStatusAsync(id, dto);
                return Ok(result);
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

        // PATCH: api/events/{id}/score
        [HttpPatch("{id:guid}/score")]
        public async Task<ActionResult<EventResponseDto>> UpdateEventScore(Guid id, EventScoreUpdateDto dto)
        {
            try
            {
                var result = await _eventService.UpdateEventScoreAsync(id, dto);
                return Ok(result);
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
        [HttpPatch("{id:guid}/soft-delete")]
        public async Task<ActionResult<EventResponseDto>> SoftDeleteEvent(Guid id)
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