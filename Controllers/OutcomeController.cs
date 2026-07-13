using Microsoft.AspNetCore.Mvc;
using API.DTOs;
using API.Interfaces;

namespace API.Controllers
{
    [ApiController]
    [Route("api/events/{eventId:int}/outcomes")]
    public class OutcomesController : ControllerBase
    {
        private readonly IOutcomeService _outcomeService;

        public OutcomesController(IOutcomeService outcomeService)
        {
            _outcomeService = outcomeService;
        }

        // GET: api/events/{eventId}/outcomes
        // Получить все этапы события
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OutcomeResponseDto>>> GetOutcomes(int eventId)
        {
            try
            {
                var result = await _outcomeService.GetOutcomesByEventIdAsync(eventId);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/events/{eventId}/outcomes
        // Добавить новый этап события
        [HttpPost]
        public async Task<ActionResult<OutcomeResponseDto>> CreateOutcome(int eventId, OutcomeCreateDto dto)
        {
            try
            {
                var result = await _outcomeService.CreateOutcomeAsync(eventId, dto);
                return CreatedAtAction(nameof(GetOutcomes), new { eventId = result.EventId }, result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}