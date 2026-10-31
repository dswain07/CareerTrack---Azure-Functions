using CareerTrack.Data.Data;
using CareerTrack.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CareerTrack.Data.Repositories
{
    public class JobApplicationRepository
    {
        private readonly CareerTrackDbContext _context;
        public JobApplicationRepository(CareerTrackDbContext context)
        {
            _context = context;
        }
        public async Task<List<JobApplication>> GetAllAsync()
        {
            return await _context.JobApplications.ToListAsync();
        }
        public async Task<JobApplication?> GetByIdAsync(int id)
        {
            return await _context.JobApplications.FindAsync(id);
        }
        public async Task<JobApplication> AddAsync(JobApplication jobApplication)
        {
            await _context.JobApplications.AddAsync(jobApplication);
            await _context.SaveChangesAsync();
            return jobApplication;
        }
        public async Task<JobApplication?> UpdateAsync(JobApplication jobApplication)
        {
            var existingJobApplication = await _context.JobApplications.FindAsync(jobApplication.Id);
            if (existingJobApplication == null)
            {
                return null;
            }
            existingJobApplication.JobTitle = jobApplication.JobTitle;
            existingJobApplication.DateApplied = jobApplication.DateApplied;
            existingJobApplication.Location = jobApplication.Location;
            existingJobApplication.Salary = jobApplication.Salary;
            existingJobApplication.Notes = jobApplication.Notes;
            existingJobApplication.CompanyId = jobApplication.CompanyId;
            existingJobApplication.ApplicationStatusId = jobApplication.ApplicationStatusId;
            existingJobApplication.JobUrl = jobApplication.JobUrl;
            await _context.SaveChangesAsync();
            return existingJobApplication;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var jobApplication = await _context.JobApplications.FindAsync(id);
            if (jobApplication == null)
            {
                return false;
            }
            _context.JobApplications.Remove(jobApplication);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
