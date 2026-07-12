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
    public class OutcomeRepository : IOutcomeRepository
    {
        private readonly AppDbContext _context; 

        public OutcomeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Outcome>> GetByEventIdAsync(int eventId)
        {
            return await _context.Outcomes
                .Where(o => o.EventId == eventId && !o.IsDeleted)
                .ToListAsync();
        }

        public async Task<Outcome> GetByIdAsync(int outcomeId)
        {
            return await _context.Outcomes
                .FirstOrDefaultAsync(o => o.OutcomeId == outcomeId && !o.IsDeleted);
        }

        public async Task<Outcome> AddAsync(Outcome outcome)
        {
            outcome.IsDeleted = false;

            await _context.Outcomes.AddAsync(outcome);
            await _context.SaveChangesAsync();

            return outcome;
        }

        public async Task<Outcome> SoftDeleteAsync(int outcomeId)
        {
            var target = await _context.Outcomes.FindAsync(outcomeId);
            if (target == null)
                return null;

            target.IsDeleted = true;
            await _context.SaveChangesAsync();
            return target;
        }
    }
}