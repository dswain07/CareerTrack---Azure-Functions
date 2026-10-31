using CareerTrack.Data.Models;
using CareerTrack.Data.Repositories;

namespace CareerTrack.Business.Services
{
    public class InterviewService
    {
        private readonly InterviewRepository _interviewRepository;
        private readonly JobApplicationRepository _jobApplicationRepository;

        public InterviewService(
            InterviewRepository interviewRepository,
            JobApplicationRepository jobApplicationRepository)
        {
            _interviewRepository = interviewRepository;
            _jobApplicationRepository = jobApplicationRepository;
        }
        public async Task<List<Interview>> GetAllAsync()
        {
            return await _interviewRepository.GetAllAsync();
        }

        public async Task<Interview?> GetByIdAsync(int id)
        {
            return await _interviewRepository.GetByIdAsync(id);
        }

        public async Task<Interview> AddAsync(Interview interview)
        {
            var application = await _jobApplicationRepository.GetByIdAsync(interview.JobApplicationId);

            if (application == null)
            {
                throw new ArgumentException(
                    "The selected job application does not exist.");
            }

            return await _interviewRepository.AddAsync(interview);
        }

        public async Task<Interview?> UpdateAsync(Interview interview)
        {
            var application = await _jobApplicationRepository.GetByIdAsync(interview.JobApplicationId);

            if (application == null)
            {
                throw new ArgumentException(
                    "The selected job application does not exist.");
            }

            return await _interviewRepository.UpdateAsync(interview);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _interviewRepository.DeleteAsync(id);
        }
    }
}
