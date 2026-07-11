using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;

namespace API.Interfaces
{
    public interface ISportService
    {
        // ============ READ ============
        Task<IEnumerable<SportResponseDto>> GetAllSportsAsync();
    }
}