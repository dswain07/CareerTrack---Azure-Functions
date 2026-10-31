using CareerTrack.Data.Models;
using CareerTrack.Data.Repositories;

namespace CareerTrack.Business.Services
{
    public class CompanyService
    {
        private readonly CompanyRepository _companyRepository;
        private readonly JobApplicationRepository _jobApplicationRepository;

        public CompanyService(CompanyRepository companyRepository, JobApplicationRepository jobApplicationRepository)
        {
            _companyRepository = companyRepository;
            _jobApplicationRepository = jobApplicationRepository;
        }
        public async Task<List<Company>> GetAllAsync()
        {
            return await _companyRepository.GetAllAsync();
        }
        public async Task<Company?> GetByIdAsync(int id)
        {
            return await _companyRepository.GetByIdAsync(id);
        }
        public async Task<Company> AddAsync(Company company)
        {
            if (string.IsNullOrWhiteSpace(company.Name))
            {
                throw new ArgumentException("Company name is required.");
            }

            var companies = await _companyRepository.GetAllAsync();

            bool duplicateExists = companies.Any(c => string.Equals(c.Name.Trim(), company.Name.Trim(), StringComparison.OrdinalIgnoreCase));

            if (duplicateExists)
            {
                throw new InvalidOperationException("Company already exists.");
            }

            company.Name = company.Name.Trim();

            return await _companyRepository.AddAsync(company);
        }
        public async Task<Company?> UpdateAsync(Company company)
        {
            if (string.IsNullOrWhiteSpace(company.Name))
            {
                throw new ArgumentException("Company name is required.");
            }

            var existingCompanies = await _companyRepository.GetAllAsync();

            bool duplicateExists = existingCompanies.Any(c => c.Id != company.Id && string.Equals(c.Name.Trim(), company.Name.Trim(), StringComparison.OrdinalIgnoreCase));

            if (duplicateExists)
            {
                throw new InvalidOperationException("Company already exists.");
            }

            company.Name = company.Name.Trim();

            return await _companyRepository.UpdateAsync(company);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var applications = await _jobApplicationRepository.GetAllAsync();

            bool hasApplications = applications.Any(a => a.CompanyId == id);

            if (hasApplications)
            {
                throw new InvalidOperationException("Cannot delete a company with existing job applications.");
            }

            return await _companyRepository.DeleteAsync(id);
        }
    }
}
