using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;

namespace API.Interfaces
{
    public interface ISportRepository
    {
        // ============ READ ============
        Task<IEnumerable<Sport>> GetAllAsync();
        Task<Sport> GetByIdAsync(int sportId);
    }
}