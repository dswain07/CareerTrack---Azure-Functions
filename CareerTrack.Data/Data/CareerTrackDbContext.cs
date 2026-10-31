using CareerTrack.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CareerTrack.Data.Data
{
    public class CareerTrackDbContext : DbContext
    {
        public CareerTrackDbContext(DbContextOptions<CareerTrackDbContext> options) : base(options)
        {

        }

        public DbSet<Company> Companies { get; set; }

        public DbSet<JobApplication> JobApplications { get; set; }

        public DbSet<Interview> Interviews { get; set; }

        public DbSet<Contact> Contacts { get; set; }

        public DbSet<ApplicationStatus> ApplicationStatuses { get; set; }
    }
}
