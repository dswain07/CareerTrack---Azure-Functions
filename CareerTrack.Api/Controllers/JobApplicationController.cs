using CareerTrack.Api.ServiceModels;
using CareerTrack.Business.Services;
using CareerTrack.Data.Models;
using Microsoft.AspNetCore.Mvc;
using CareerTrack.Business.Models;

namespace CareerTrack.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobApplicationsController : ControllerBase
    {
        private readonly JobApplicationService _jobApplicationService;
        private readonly BlobReportService _blobReportService;

        public JobApplicationsController(JobApplicationService jobApplicationService, BlobReportService blobReportService)
        {
            _jobApplicationService = jobApplicationService;
            _blobReportService = blobReportService;
        }

        [HttpGet]
        public async Task<ActionResult<List<JobApplicationModel>>> GetAll()
        {
            var applications = await _jobApplicationService.GetAllAsync();

            var models = applications.Select(a => new JobApplicationModel
            {
                Id = a.Id,
                JobTitle = a.JobTitle,
                CompanyId = a.CompanyId,
                ApplicationStatusId = a.ApplicationStatusId,
                DateApplied = a.DateApplied,
                Location = a.Location,
                JobUrl = a.JobUrl,
                Salary = a.Salary,
                Notes = a.Notes
            }).ToList();

            return Ok(models);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<JobApplicationModel>> GetById(int id)
        {
            var application = await _jobApplicationService.GetByIdAsync(id);

            if (application == null)
            {
                return NotFound();
            }

            var model = new JobApplicationModel
            {
                Id = application.Id,
                JobTitle = application.JobTitle,
                CompanyId = application.CompanyId,
                ApplicationStatusId = application.ApplicationStatusId,
                DateApplied = application.DateApplied,
                Location = application.Location,
                JobUrl = application.JobUrl,
                Salary = application.Salary,
                Notes = application.Notes
            };

            return Ok(model);
        }

        [HttpPost]
        public async Task<ActionResult<JobApplicationModel>> Create(
            [FromBody] JobApplicationModel model)
        {
            var application = new JobApplication
            {
                JobTitle = model.JobTitle,
                CompanyId = model.CompanyId,
                ApplicationStatusId = model.ApplicationStatusId,
                DateApplied = model.DateApplied,
                Location = model.Location,
                JobUrl = model.JobUrl,
                Salary = model.Salary,
                Notes = model.Notes
            };

            try
            {
                var created = await _jobApplicationService.AddAsync(application);

                model.Id = created.Id;

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = model.Id },
                    model);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<JobApplicationModel>> Update(
            int id, [FromBody] JobApplicationModel model)
        {
            if (id != model.Id)
            {
                return BadRequest("Job application ID mismatch.");
            }

            var application = new JobApplication
            {
                Id = id,
                JobTitle = model.JobTitle,
                CompanyId = model.CompanyId,
                ApplicationStatusId = model.ApplicationStatusId,
                DateApplied = model.DateApplied,
                Location = model.Location,
                JobUrl = model.JobUrl,
                Salary = model.Salary,
                Notes = model.Notes
            };

            try
            {
                var updated = await _jobApplicationService.UpdateAsync(application);

                if (updated == null)
                {
                    return NotFound();
                }

                return Ok(model);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool deleted = await _jobApplicationService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("application-score")]
        public async Task<ActionResult<ApplicationScoreResult>> CalculateApplicationScore([FromQuery] string status, [FromQuery] int daysSinceApplied, [FromQuery] decimal salary)
        {
            if (string.IsNullOrWhiteSpace(status) || daysSinceApplied < 0 || salary < 0)
            {
                return BadRequest("Invalid application score parameters.");
            }

            try
            {
                var result = await _jobApplicationService.CalculateApplicationScoreAsync(
                    status,
                    daysSinceApplied,
                    salary);

                return Ok(result);
            }
            catch (HttpRequestException)
            {
                return StatusCode(502, "Unable to communicate with Azure Functions.");
            }
            catch (TaskCanceledException)
            {
                return StatusCode(504, "Azure Functions request timed out.");
            }
        }

        [HttpGet("follow-up-recommendation")]
        public async Task<ActionResult<FollowUpRecommendationResult>> GenerateFollowUpRecommendation([FromQuery] string status, [FromQuery] int daysSinceApplied)
        {
            if (string.IsNullOrWhiteSpace(status) || daysSinceApplied < 0)
            {
                return BadRequest("Invalid follow-up recommendation parameters.");
            }

            try
            {
                var result = await _jobApplicationService.GenerateFollowUpRecommendationAsync(
                    status,
                    daysSinceApplied);

                return Ok(result);
            }
            catch (HttpRequestException)
            {
                return StatusCode(502, "Unable to communicate with Azure Functions.");
            }
            catch (TaskCanceledException)
            {
                return StatusCode(504, "Azure Functions request timed out.");
            }
        }


        [HttpPost("{id:int}/upload-report")]
        public async Task<IActionResult> UploadApplicationReport(int id)
        {
            if (id <= 0)
            {
                return BadRequest("A valid application ID is required.");
            }

            try
            {
                string? blobName =
                    await _blobReportService.UploadApplicationReportAsync(id);

                if (blobName == null)
                {
                    return NotFound("Job application not found.");
                }

                return Ok(new
                {
                    Message = "Application report uploaded successfully.",
                    ApplicationId = id,
                    BlobName = blobName,
                    ContainerName = "careertrack-reports"
                });
            }
            catch (Azure.RequestFailedException)
            {
                return StatusCode(
                    502,
                    "Unable to upload the report to Azure Blob Storage.");
            }
        }

    }
}
