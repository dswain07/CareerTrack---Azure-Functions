using CareerTrack.Data.Data;
using CareerTrack.Data.Models;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CareerTrack.Data.Repositories
{
    public class CompanyRepository
    {
        private readonly CareerTrackDbContext _context;
        public CompanyRepository(CareerTrackDbContext context)
        {
            _context = context;
        }
        public async Task<List<Company>> GetAllAsync()
        {
            return await _context.Companies.ToListAsync();
        }
        public async Task<Company?> GetByIdAsync(int id)
        {
            return await _context.Companies.FindAsync(id);
        }
        public async Task<Company> AddAsync(Company company)
        {
            await _context.Companies.AddAsync(company);
            await _context.SaveChangesAsync();
            return company;
        }
        public async Task<Company?> UpdateAsync(Company company)
        {
            var existingCompany = await _context.Companies.FindAsync(company.Id);
            if (existingCompany == null)
            {
                return null;
            }
            existingCompany.Name = company.Name;
            existingCompany.Industry = company.Industry;
            existingCompany.Location = company.Location;
            await _context.SaveChangesAsync();
            return existingCompany;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return false;
            }
            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
