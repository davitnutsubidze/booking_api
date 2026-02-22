using AutoMapper;
using Booking_API.data;
using Booking_API.Models;
using Booking_API.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Booking_API.Controllers
{
    [Route("api/companyEmployees-employees")]
    [ApiController]
    //[Authorize(Roles = "Customer, Admin")]
    public class CompanyEmployeeController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public CompanyEmployeeController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<CompanyEmployeesDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse <IEnumerable<CompanyEmployeesDTO>>>> GetCompanyEmploees()
        {
            var companyEmployees = await _db.CompanyEmployees.ToListAsync();
            var dtoResponseCompanyEmployees = _mapper.Map<List<CompanyEmployeesDTO>>(companyEmployees);
            var response = ApiResponse<IEnumerable<CompanyEmployeesDTO>>.Ok(dtoResponseCompanyEmployees, "Employees retriev successfully");
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<CompanyEmployeesDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<CompanyEmployeesDTO>>> GetCompanyEmployeeById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return NotFound(ApiResponse<object>.NotFound("CompanyEmployee Id must be greater than 0"));
                }
                var companyEmployees = await _db.CompanyEmployees.FirstOrDefaultAsync(x => x.Id == id);
                if (companyEmployees == null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"CompanyEmployee with Id {id} was not found"));
                }

                return Ok(ApiResponse<CompanyEmployeesDTO>.Ok(_mapper.Map<CompanyEmployeesDTO>(companyEmployees), "Request successful"));

            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(500, $"An error occurred while Creating the companyEmployees: {ex.Message}", ex.Message);
                return StatusCode(500, errorResponse);
            }

        }

        [HttpPost()]
        [ProducesResponseType(typeof(ApiResponse<CompanyEmployeesDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<CompanyEmployeesDTO>>> CreateCompanyEmployee(CompanyEmployeesCreateDTO companyEmployeesDTO)
        {
            try
            {
                if (companyEmployeesDTO == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Company Employees Data is required"));
                }
                var companyExists = await _db.Company.FirstOrDefaultAsync(u=>u.Id == companyEmployeesDTO.CompanyId);

                if (companyExists == null)
                {
                    return Conflict(ApiResponse<object>.Conflict($"A companyEmployees with the ID '{companyEmployeesDTO.CompanyId}' doesn not exist"));
                }

                CompanyEmployees companyEmployees = _mapper.Map<CompanyEmployees>(companyEmployeesDTO);
                companyEmployees.CreatedDate = new DateTime();

                await _db.CompanyEmployees.AddAsync(companyEmployees);
                await _db.SaveChangesAsync();

                
                var responseDTO = _mapper.Map<CompanyEmployeesDTO>(companyEmployees);
                var response = ApiResponse<CompanyEmployeesDTO>.CreatedAt(responseDTO, "CompanyEmployees created Successfully");
                return CreatedAtAction(nameof(CreateCompanyEmployee), new { id = companyEmployees.Id }, response);

            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(500, $"An error occurred while Creating the companyEmployees: {ex.Message}", ex.Message);
                return StatusCode(500, errorResponse);
            }

        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<CompanyEmployeesDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<CompanyEmployeesDTO>>> UpdateCompanyEmployee(int id, CompanyEmployeesUpdateDTO companyEmployeesDTO)
        {
            try
            {
                if (companyEmployeesDTO == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Company Employees Data is required"));

                }

                if (id != companyEmployeesDTO.Id)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Company Employees Id is URL does not match COmpany Id in request body"));
                }

                var existCompanyEmployee = await _db.CompanyEmployees.FirstOrDefaultAsync(u => u.Id == id);

                if (existCompanyEmployee == null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"Company Employees with Id {id} was not found"));
                }

                var companyExists = await _db.Company.FirstOrDefaultAsync(u => u.Id == companyEmployeesDTO.CompanyId);

                if (companyExists == null)
                {
                    return Conflict(ApiResponse<object>.Conflict($"A companyEmployees with the ID '{companyEmployeesDTO.CompanyId}' doesn not exist"));
                }

                _mapper.Map(companyEmployeesDTO, existCompanyEmployee);
                existCompanyEmployee.UpdatedDate = new DateTime(2026, 2, 9, 12, 20, 0, DateTimeKind.Utc);

                await _db.SaveChangesAsync();

                var response = ApiResponse<CompanyEmployeesDTO>.Ok(_mapper.Map<CompanyEmployeesDTO>(existCompanyEmployee), "CompanyEmployees updated successfully");
                return Ok(response);

            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(500, $"An error occurred while Creating the company Employees: {ex.Message}", ex.Message);
                return StatusCode(500, errorResponse);
            }

        }



        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<object>>> DeleteCompanyEmployee(int id)
        {
            try
            {

                var existCompanyEmployee = await _db.CompanyEmployees.FirstOrDefaultAsync(u => u.Id == id);

                if (existCompanyEmployee == null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"CompanyEmployees with Id {id} was not found"));
                }


                _db.CompanyEmployees.Remove(existCompanyEmployee);
                await _db.SaveChangesAsync();

                var response = ApiResponse<object>.NoContent("CompanyEmployees deleted successfully");
                return Ok(response);

            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(500, $"An error occurred while Creating the companyEmployees: {ex.Message}", ex.Message);
                return StatusCode(500, errorResponse);
            }

        }
    }
}
