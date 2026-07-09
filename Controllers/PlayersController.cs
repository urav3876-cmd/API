using Microsoft.AspNetCore.Mvc;
using API.DTOs;
using API.Services;
using API.Interfaces;

namespace API.Controllers
{
    [ApiController]
    [Route("api/players")]
    public class PlayersController : ControllerBase
    {
        private readonly IPlayerService _playerService;

        public PlayersController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        // POST: api/players
        [HttpPost]
        public async Task<ActionResult<PlayerResponseDto>> CreatePlayer(PlayerCreateDto dto)
        {
            try
            {
                var result = await _playerService.CreatePlayerAsync(dto);
                return CreatedAtAction(nameof(GetPlayer), new { id = result.PlayerId }, result);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        // GET: api/players/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PlayerResponseDto>> GetPlayer(int id)
        {
            var result = await _playerService.GetPlayerAsync(id);

            if (result == null)
                return NotFound($"Player with ID {id} not found");

            return Ok(result);
        }

        // PUT: api/players/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<PlayerResponseDto>> UpdatePlayer(int id, PlayerUpdateDto dto)
        {
            try
            {
                var result = await _playerService.UpdatePlayerAsync(id, dto);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }
    }
}