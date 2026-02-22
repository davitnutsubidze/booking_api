using AutoMapper;
using Booking_API.data;
using Booking_API.Models;
using Booking_API.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Booking_API.Controllers
{
    [Route("api/company")]
    [ApiController]
    [Authorize(Roles = "Customer, Admin")]
    public class CompanyController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public CompanyController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<CompanyDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse <IEnumerable<CompanyDTO>>>> GetCompanies()
        {
            var companies = await _db.Company.ToListAsync();
            var dtoResponseCompany = _mapper.Map<List<CompanyDTO>>(companies);
            var response = ApiResponse<IEnumerable<CompanyDTO>>.Ok(dtoResponseCompany, "Companies retriev successfully");
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<CompanyDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<CompanyDTO>>> GetCompanyById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return NotFound(ApiResponse<object>.NotFound("Company Id must be greater than 0"));
                }
                var company = await _db.Company.FirstOrDefaultAsync(x => x.Id == id);
                if (company == null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"Company with Id {id} was not found"));
                }

                return Ok(ApiResponse<CompanyDTO>.Ok(_mapper.Map<CompanyDTO>(company), "Request successful"));

            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(500, $"An error occurred while Creating the company: {ex.Message}", ex.Message);
                return StatusCode(500, errorResponse);
            }

        }

        [HttpPost()]
        [ProducesResponseType(typeof(ApiResponse<CompanyDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<CompanyDTO>>> CreateCompany(CompanyCreateDTO companyDTO)
        {
            try
            {
                if (companyDTO == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Company Data is required"));
                }
                var duplicateCompany = await _db.Company.FirstOrDefaultAsync(u=>u.Name.ToLower() == companyDTO.Name.ToLower());

                if (duplicateCompany != null)
                {
                    return Conflict(ApiResponse<object>.Conflict($"A company with the name '{companyDTO.Name}' already exist"));
                }

                Company company = _mapper.Map<Company>(companyDTO);

                await _db.Company.AddAsync(company);
                await _db.SaveChangesAsync();

                
                var responseDTO = _mapper.Map<CompanyDTO>(company);
                var response = ApiResponse<CompanyDTO>.CreatedAt(responseDTO, "Company created Successfully");
                return CreatedAtAction(nameof(CreateCompany), new { id = company.Id }, response);

            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(500, $"An error occurred while Creating the company: {ex.Message}", ex.Message);
                return StatusCode(500, errorResponse);
            }

        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<CompanyDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<CompanyDTO>>> UpdateCompany(int id, CompanyUpdateDTO companyDTO)
        {
            try
            {
                if (companyDTO == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Company Data is required"));

                }

                if (id != companyDTO.Id)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Company Id is URL does not match COmpany Id in request body"));
                }

                var existCompany = await _db.Company.FirstOrDefaultAsync(u => u.Id == id);

                if (existCompany == null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"Company with Id {id} was not found"));
                }

                var duplicateCompany = await _db.Company.FirstOrDefaultAsync(u => u.Name.ToLower() == companyDTO.Name.ToLower() &&
                u.Id != id);
                if (duplicateCompany != null)
                {
                    return Conflict(ApiResponse<object>.Conflict($"A company with the name '{companyDTO.Name}' already exist"));
                }

                _mapper.Map(companyDTO, existCompany);
                existCompany.UpdatedDate = new DateTime(2026, 2, 9, 12, 20, 0, DateTimeKind.Utc);

                await _db.SaveChangesAsync();

                var response = ApiResponse<CompanyDTO>.Ok(_mapper.Map<CompanyDTO>(companyDTO), "Company updated successfully");
                return Ok(response);

            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(500, $"An error occurred while Creating the company: {ex.Message}", ex.Message);
                return StatusCode(500, errorResponse);
            }

        }



        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<object>>> DeleteCompany(int id)
        {
            try
            {

                var existCompany = await _db.Company.FirstOrDefaultAsync(u => u.Id == id);

                if (existCompany == null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"Company with Id {id} was not found"));
                }


                _db.Company.Remove(existCompany);
                await _db.SaveChangesAsync();

                var response = ApiResponse<object>.NoContent("Company deleted successfully");
                return Ok(response);

            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(500, $"An error occurred while Creating the company: {ex.Message}", ex.Message);
                return StatusCode(500, errorResponse);
            }

        }
    }
}
