using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using API.Entities;
using API.Data;
using API.Interfaces;

namespace API.Repositories
{
    public class SportRepository : ISportRepository
    {
        private readonly EventDbContext _context;

        public SportRepository(EventDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Sport>> GetAllAsync()
        {
            return await _context.Sports
                .Where(s => !s.IsDeleted)
                .OrderBy(s => s.SportName)
                .ToListAsync();
        }

        public async Task<Sport> GetByIdAsync(int sportId)
        {
            return await _context.Sports
                .FirstOrDefaultAsync(s => s.SportId == sportId && !s.IsDeleted);
        }

        // Добавить метод для поиска по имени
        public async Task<Sport> GetByNameAsync(string sportName)
        {
            return await _context.Sports
                .FirstOrDefaultAsync(s => s.SportName == sportName && !s.IsDeleted);
        }
    }
}