using CareerTrack.Data.Data;
using CareerTrack.Data.Models;
using CareerTrack.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CareerTrack.Business.Services
{
    public class ApplicationStatusRepository
    {
        private readonly CareerTrackDbContext _context;

        public ApplicationStatusRepository(CareerTrackDbContext context)
        {
            _context = context;
        }

        public async Task<ApplicationStatus?> GetByIdAsync(int id)
        {
            return await _context.ApplicationStatuses.FindAsync(id);
        }
    }
}