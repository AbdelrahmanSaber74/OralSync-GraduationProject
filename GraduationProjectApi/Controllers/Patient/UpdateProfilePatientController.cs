using IdentityManagerServerApi.Data;
using IdentityManagerServerApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedClassLibrary.DTOs;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GraduationProjectApi.Controllers.Students
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UpdateProfilePatientController : ControllerBase
    {
        private readonly AppDbContext _db;

        public UpdateProfilePatientController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> Get(PatientDTO patientDTO)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userRole))
                return BadRequest(new { StatusCode = 400, Message = "User ID or Role not found in claims." });

            if (userRole == "Patient")
            {
                var patient = await _db.Patients.FirstOrDefaultAsync(x => x.UserId == userId);

                if (patient == null)
                {
                    return NotFound(new { StatusCode = 404, Message = "Patient not found." });
                }

                // Example update to GPA
                patient.FirstName = patientDTO.FirstName;
               

                _db.Update(patient);
                await _db.SaveChangesAsync();

                return Ok(new { StatusCode = 200, Message = "Patient profile updated successfully." });
            }


            return Forbid(); // User is not a Patient, so forbid the action.

        }
    }
}
