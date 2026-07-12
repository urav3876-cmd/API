
using API.DTOs;
using API.Entities;
using API.Interfaces;

namespace API.Services
{
    public class SportService : ISportService
    {
        private readonly ISportRepository _sportRepository;

        public SportService(ISportRepository sportRepository)
        {
            _sportRepository = sportRepository;
        }

        // ============ READ ============
        public async Task<IEnumerable<SportResponseDto>> GetAllSportsAsync()
        {
            var sports = await _sportRepository.GetAllAsync();

            return sports.Select(sport => new SportResponseDto
            {
                SportId = sport.SportId,
                SportName = sport.SportName
            });
        }
    }
}