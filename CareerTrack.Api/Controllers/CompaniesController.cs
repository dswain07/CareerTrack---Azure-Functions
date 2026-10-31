using CareerTrack.Api.ServiceModels;
using CareerTrack.Business.Services;
using CareerTrack.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CareerTrack.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly CompanyService _companyService;

        public CompaniesController(CompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CompanyModel>>> GetAll()
        {
            var companies = await _companyService.GetAllAsync();

            var models = companies.Select(c => new CompanyModel
            {
                Id = c.Id,
                Name = c.Name,
                Industry = c.Industry,
                Location = c.Location
            }).ToList();

            return Ok(models);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyModel>> GetById(int id)
        {
            var company = await _companyService.GetByIdAsync(id);

            if (company == null)
            {
                return NotFound();
            }

            var model = new CompanyModel
            {
                Id = company.Id,
                Name = company.Name,
                Industry = company.Industry,
                Location = company.Location
            };

            return Ok(model);
        }

        [HttpPost]
        public async Task<ActionResult<CompanyModel>> Create(
            [FromBody] CompanyModel model)
        {
            var company = new Company
            {
                Name = model.Name,
                Industry = model.Industry,
                Location = model.Location
            };

            try
            {
                var createdCompany = await _companyService.AddAsync(company);

                var result = new CompanyModel
                {
                    Id = createdCompany.Id,
                    Name = createdCompany.Name,
                    Industry = createdCompany.Industry,
                    Location = createdCompany.Location
                };

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = result.Id },
                    result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CompanyModel>> Update(
            int id, [FromBody] CompanyModel model)
        {
            if (id != model.Id)
            {
                return BadRequest("Company ID mismatch.");
            }

            var company = new Company
            {
                Id = id,
                Name = model.Name,
                Industry = model.Industry,
                Location = model.Location
            };

            try
            {
                var updatedCompany = await _companyService.UpdateAsync(company);

                if (updatedCompany == null)
                {
                    return NotFound();
                }

                var result = new CompanyModel
                {
                    Id = updatedCompany.Id,
                    Name = updatedCompany.Name,
                    Industry = updatedCompany.Industry,
                    Location = updatedCompany.Location
                };

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                bool deleted = await _companyService.DeleteAsync(id);

                if (!deleted)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

    }
}
