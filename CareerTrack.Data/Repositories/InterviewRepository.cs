using CareerTrack.Data.Data;
using CareerTrack.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CareerTrack.Data.Repositories
{
    public class InterviewRepository
    {
        private readonly CareerTrackDbContext _context;
        public InterviewRepository(CareerTrackDbContext context)
        {
            _context = context;
        }
        public async Task<List<Interview>> GetAllAsync()
        {
            return await _context.Interviews.ToListAsync();
        }
        public async Task<Interview?> GetByIdAsync(int id)
        {
            return await _context.Interviews.FindAsync(id);
        }
        public async Task<Interview> AddAsync(Interview interview)
        {
            await _context.Interviews.AddAsync(interview);
            await _context.SaveChangesAsync();
            return interview;
        }
        public async Task<Interview?> UpdateAsync(Interview interview)
        {
            var existingInterview = await _context.Interviews.FindAsync(interview.Id);
            if (existingInterview == null)
            {
                return null;
            }
            existingInterview.InterviewType = interview.InterviewType;
            existingInterview.InterviewDate = interview.InterviewDate;
            existingInterview.Location = interview.Location;
            existingInterview.Outcome = interview.Outcome;
            existingInterview.Notes = interview.Notes;
            existingInterview.JobApplicationId = interview.JobApplicationId;
            await _context.SaveChangesAsync();
            return existingInterview;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var interview = await _context.Interviews.FindAsync(id);
            if (interview == null)
            {
                return false;
            }
            _context.Interviews.Remove(interview);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
