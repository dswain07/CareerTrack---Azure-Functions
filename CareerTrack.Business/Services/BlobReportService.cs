
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CareerTrack.Data.Repositories;
using System.Text.Json;

namespace CareerTrack.Business.Services
{
    public class BlobReportService
    {
        private readonly BlobContainerClient _containerClient;
        private readonly JobApplicationRepository _jobApplicationRepository;
        private readonly CompanyRepository _companyRepository;

        public BlobReportService(
            BlobServiceClient blobServiceClient,
            JobApplicationRepository jobApplicationRepository,
            CompanyRepository companyRepository,
            string containerName)
        {
            _containerClient =
                blobServiceClient.GetBlobContainerClient(containerName);

            _jobApplicationRepository = jobApplicationRepository;
            _companyRepository = companyRepository;
        }

        public async Task<string?> UploadApplicationReportAsync(int applicationId)
        {
            var application =
                await _jobApplicationRepository.GetByIdAsync(applicationId);

            if (application == null)
            {
                return null;
            }

            var company =
                await _companyRepository.GetByIdAsync(application.CompanyId);

            var report = new
            {
                ApplicationId = application.Id,
                JobTitle = application.JobTitle,
                Company = company?.Name ?? "Unknown",
                ApplicationStatusId = application.ApplicationStatusId,
                DateApplied = application.DateApplied,
                GeneratedAtUtc = DateTime.UtcNow
            };

            string json = JsonSerializer.Serialize(
                report,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });

            string blobName =
                $"application-{applicationId}-{DateTime.UtcNow:yyyyMMddHHmmssfff}.json";

            var blobClient = _containerClient.GetBlobClient(blobName);

            using var stream = new MemoryStream(
                System.Text.Encoding.UTF8.GetBytes(json));

            await blobClient.UploadAsync(
                stream,
                new BlobHttpHeaders
                {
                    ContentType = "application/json"
                });

            return blobName;
        }
    }
}
