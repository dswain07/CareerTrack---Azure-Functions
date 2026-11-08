using CareerTrack.Data.Models;
using CareerTrack.Data.Repositories;
using CareerTrack.Business.Models;
using System.Net.Http.Json;

namespace CareerTrack.Business.Services
{
    public class JobApplicationService
    {
        private readonly JobApplicationRepository _jobApplicationRepository;
        private readonly CompanyRepository _companyRepository;
        private readonly ApplicationStatusRepository _applicationStatusRepository;
        private readonly HttpClient _httpClient;

        public JobApplicationService(JobApplicationRepository jobApplicationRepository, CompanyRepository companyRepository, ApplicationStatusRepository applicationStatusRepository, HttpClient httpClient)
        {
            _jobApplicationRepository = jobApplicationRepository;
            _companyRepository = companyRepository;
            _applicationStatusRepository = applicationStatusRepository;
            _httpClient = httpClient;
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
        public async Task<ApplicationScoreResult> CalculateApplicationScoreAsync(string status, int daysSinceApplied, decimal salary)
        {
            string url =
                $"api/CalculateApplicationScore" +
                $"?status={Uri.EscapeDataString(status)}" +
                $"&daysSinceApplied={daysSinceApplied}" +
                $"&salary={salary.ToString(System.Globalization.CultureInfo.InvariantCulture)}";

            var result = await _httpClient.GetFromJsonAsync<ApplicationScoreResult>(url);

            return result ?? throw new InvalidOperationException(
                "Azure Function returned an empty application score.");
        }

        public async Task<FollowUpRecommendationResult> GenerateFollowUpRecommendationAsync(string status, int daysSinceApplied)
        {
            string url =
                $"api/GenerateFollowUpRecommendation" +
                $"?status={Uri.EscapeDataString(status)}" +
                $"&daysSinceApplied={daysSinceApplied}";

            var result = await _httpClient.GetFromJsonAsync<FollowUpRecommendationResult>(url);

            return result ?? throw new InvalidOperationException(
                "Azure Function returned an empty follow-up recommendation.");
        }
    }
}
