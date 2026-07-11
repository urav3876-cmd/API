using Microsoft.AspNetCore.Mvc;
using API.DTOs;
using API.Interfaces;

namespace API.Controllers
{
    [ApiController]
    [Route("api/sports")]
    public class SportsController : ControllerBase
    {
        private readonly ISportService _sportService;

        public SportsController(ISportService sportService)
        {
            _sportService = sportService;
        }

        // GET: api/sports
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SportResponseDto>>> GetAllSports()
        {
            var result = await _sportService.GetAllSportsAsync();
            return Ok(result);
        }
    }
}