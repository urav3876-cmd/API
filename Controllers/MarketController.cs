using Microsoft.AspNetCore.Mvc;
using API.DTOs;
using API.Interfaces;

namespace API.Controllers
{
    [ApiController]
    [Route("api/events/{eventId:guid}/markets")]
    public class MarketsController : ControllerBase
    {
        private readonly IMarketService _marketService;

        public MarketsController(IMarketService marketService)
        {
            _marketService = marketService;
        }

        // GET: api/events/{eventId}/markets/{marketId}
        // Получить маркет со всеми selection
        [HttpGet("{marketId:guid}")]
        public async Task<ActionResult<MarketResponseDto>> GetMarket(int eventId, int marketId)
        {
            try
            {
                var result = await _marketService.GetMarketByIdAsync(marketId);

                if (result.EventId != eventId)
                    return NotFound($"Market with ID {marketId} not found for event {eventId}");

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

        // POST: api/events/{eventId}/markets
        // Добавить маркет со всеми selection
        [HttpPost]
        public async Task<ActionResult<MarketResponseDto>> CreateMarket(int eventId, MarketCreateDto dto)
        {
            try
            {
                var result = await _marketService.CreateMarketAsync(eventId, dto);
                return CreatedAtAction(nameof(GetMarket), new { eventId, marketId = result.MarketId }, result);
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