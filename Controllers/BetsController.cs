using Microsoft.AspNetCore.Mvc;
using API.DTOs;
using API.Services;
using API.Interfaces;

namespace API.Controllers
{
    [ApiController]
    [Route("api/bets")]
    public class BetsController : ControllerBase
    {
        private readonly IBetService _betService;

        public BetsController(IBetService betService)
        {
            _betService = betService;
        }

        // POST: api/bets
        [HttpPost]
        public async Task<ActionResult<BetResponseDto>> CreateBet(BetCreateDto dto)
        {
            try
            {
                var result = await _betService.CreateBetAsync(dto);
                return CreatedAtAction(nameof(GetBet), new { id = result.BetId }, result);
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

        // GET: api/bets/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<BetResponseDto>> GetBet(int id)
        {
            var result = await _betService.GetBetAsync(id);

            if (result == null)
                return NotFound($"Bet with ID {id} not found");

            return Ok(result);
        }
    }
}