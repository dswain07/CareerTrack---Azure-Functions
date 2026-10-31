using CareerTrack.Api.ServiceModels;
using CareerTrack.Business.Services;
using CareerTrack.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace CareerTrack.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobApplicationsController : ControllerBase
    {
        private readonly JobApplicationService _jobApplicationService;

        public JobApplicationsController(
            JobApplicationService jobApplicationService)
        {
            _jobApplicationService = jobApplicationService;
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
    }
}
