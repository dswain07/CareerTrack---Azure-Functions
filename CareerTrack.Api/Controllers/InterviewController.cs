using CareerTrack.Api.ServiceModels;
using CareerTrack.Business.Services;
using CareerTrack.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace CareerTrack.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InterviewsController : ControllerBase
    {
        private readonly InterviewService _interviewService;

        public InterviewsController(InterviewService interviewService)
        {
            _interviewService = interviewService;
        }

        [HttpGet]
        public async Task<ActionResult<List<InterviewModel>>> GetAll()
        {
            var interviews = await _interviewService.GetAllAsync();

            var models = interviews.Select(i => new InterviewModel
            {
                Id = i.Id,
                InterviewType = i.InterviewType,
                InterviewDate = i.InterviewDate,
                Location = i.Location,
                Outcome = i.Outcome,
                Notes = i.Notes,
                JobApplicationId = i.JobApplicationId
            }).ToList();

            return Ok(models);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InterviewModel>> GetById(int id)
        {
            var interview = await _interviewService.GetByIdAsync(id);

            if (interview == null)
            {
                return NotFound();
            }

            var model = new InterviewModel
            {
                Id = interview.Id,
                InterviewType = interview.InterviewType,
                InterviewDate = interview.InterviewDate,
                Location = interview.Location,
                Outcome = interview.Outcome,
                Notes = interview.Notes,
                JobApplicationId = interview.JobApplicationId
            };

            return Ok(model);
        }

        [HttpPost]
        public async Task<ActionResult<InterviewModel>> Create(
            [FromBody] InterviewModel model)
        {
            var interview = new Interview
            {
                InterviewType = model.InterviewType,
                InterviewDate = model.InterviewDate,
                Location = model.Location,
                Outcome = model.Outcome,
                Notes = model.Notes,
                JobApplicationId = model.JobApplicationId
            };

            try
            {
                var created = await _interviewService.AddAsync(interview);

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
        public async Task<ActionResult<InterviewModel>> Update(
            int id, [FromBody] InterviewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest("Interview ID mismatch.");
            }

            var interview = new Interview
            {
                Id = id,
                InterviewType = model.InterviewType,
                InterviewDate = model.InterviewDate,
                Location = model.Location,
                Outcome = model.Outcome,
                Notes = model.Notes,
                JobApplicationId = model.JobApplicationId
            };

            try
            {
                var updated = await _interviewService.UpdateAsync(interview);

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
            bool deleted = await _interviewService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}

