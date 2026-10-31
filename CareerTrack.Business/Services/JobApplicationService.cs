using CareerTrack.Data.Models;
using CareerTrack.Data.Repositories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CareerTrack.Business.Services
{
    public class JobApplicationService
    {
        private readonly JobApplicationRepository _jobApplicationRepository;
        private readonly CompanyRepository _companyRepository;
        private readonly ApplicationStatusRepository _applicationStatusRepository;

        public JobApplicationService(JobApplicationRepository jobApplicationRepository, CompanyRepository companyRepository, ApplicationStatusRepository applicationStatusRepository)
        {
            _jobApplicationRepository = jobApplicationRepository;
            _companyRepository = companyRepository;
            _applicationStatusRepository = applicationStatusRepository;
        }
        public async Task<List<JobApplication>> GetAllAsync()
        {
            return await _jobApplicationRepository.GetAllAsync();
        }

        public async Task<JobApplication?> GetByIdAsync(int id)
        {
            return await _jobApplicationRepository.GetByIdAsync(id);
        }
        public async Task<JobApplication> AddAsync(JobApplication jobApplication)
        {
            if (jobApplication.Salary.HasValue && jobApplication.Salary.Value < 0)
            {
                throw new ArgumentException("Salary cannot be negative.");
            }

            var company = await _companyRepository.GetByIdAsync(jobApplication.CompanyId);

            if (company == null)
            {
                throw new ArgumentException("The selected company does not exist.");
            }

            var status = await _applicationStatusRepository.GetByIdAsync(jobApplication.ApplicationStatusId);

            if (status == null)
            {
                throw new ArgumentException("The selected application status does not exist.");
            }

            return await _jobApplicationRepository.AddAsync(jobApplication);
        }
        public async Task<JobApplication?> UpdateAsync(JobApplication jobApplication)
        {
            if (jobApplication.Salary.HasValue && jobApplication.Salary.Value < 0)
            {
                throw new ArgumentException("Salary cannot be negative.");
            }

            var company = await _companyRepository.GetByIdAsync(jobApplication.CompanyId);

            if (company == null)
            {
                throw new ArgumentException("The selected company does not exist.");
            }

            var status = await _applicationStatusRepository.GetByIdAsync(jobApplication.ApplicationStatusId);

            if (status == null)
            {
                throw new ArgumentException("The selected application status does not exist.");
            }

            return await _jobApplicationRepository.UpdateAsync(jobApplication);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            return await _jobApplicationRepository.DeleteAsync(id);
        }
    }
}
