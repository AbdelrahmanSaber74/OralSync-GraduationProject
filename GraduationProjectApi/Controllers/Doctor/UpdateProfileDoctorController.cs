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
    public class UpdateProfileDoctorController : ControllerBase
    {
        private readonly AppDbContext _db;

        public UpdateProfileDoctorController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> Get(DoctorDTO doctorDTO)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userRole))
                return BadRequest(new { StatusCode = 400, Message = "User ID or Role not found in claims." });

            if (userRole == "Doctor")
            {
                var doctor = await _db.Doctors.FirstOrDefaultAsync(x => x.UserId == userId);
                var userDoctor = await _db.Users.FirstOrDefaultAsync(x => x.Id == userId);

                if (doctor == null)
                {
                    return NotFound(new { StatusCode = 404, Message = "Doctor not found." });
                }

                // update Doctor In table Doctor
                doctor.GPA = doctorDTO.GPA;
                doctor.PhoneNumber = doctorDTO.PhoneNumber;
                doctor.FirstName = doctorDTO.FirstName;
                doctor.LastName = doctorDTO.LastName;
                doctor.Email = doctorDTO.Email;
                doctor.IsMale = doctorDTO.IsMale;
                doctor.UniversityName = doctorDTO.UniversityName;
                doctor.ClinicAddress = doctorDTO.ClinicAddress;
                doctor.ClinicNumber = doctorDTO.ClinicNumber;
                doctor.InsuranceCompanies = doctorDTO.InsuranceCompanies;
                doctor.Certificates = doctorDTO.Certificates;
                doctor.BirthDate = doctorDTO.BirthDate;
                doctor.GraduationDate = doctorDTO.GraduationDate;



                // update Doctor In table Users
                userDoctor.Name = doctorDTO.FirstName+" "+doctorDTO.LastName;
                userDoctor.UserName = doctorDTO.Email;
                userDoctor.NormalizedUserName = doctorDTO.Email.ToUpper();
                userDoctor.Email = doctorDTO.Email;
                userDoctor.NormalizedEmail = doctorDTO.Email.ToUpper();
                userDoctor.PhoneNumber = doctorDTO.PhoneNumber;



                _db.Update(doctor);
                await _db.SaveChangesAsync();

                return Ok(new { StatusCode = 200, Message = "Doctor profile updated successfully." });
            }


            return Forbid(); // User is not a Doctor, so forbid the action.

        }
    }
}
