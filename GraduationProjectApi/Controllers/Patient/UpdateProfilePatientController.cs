using IdentityManagerServerApi.Data;
using IdentityManagerServerApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedClassLibrary.DTOs;
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

        [HttpPut]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> Put(PatientDTO patientDTO)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new { StatusCode = 400, MessageEn = "User ID not found in claims.", MessageAr = "معرف المستخدم غير موجود في البيانات." });
            }

            var patient = await _db.Patients.FirstOrDefaultAsync(x => x.UserId == userId);
            var userPatient = await _db.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (patient == null)
            {
                return NotFound(new { StatusCode = 404, MessageEn = "Patient not found.", MessageAr = "لم يتم العثور على المريض." });
            }

            if (!User.IsInRole("Patient"))
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { StatusCode = 403, MessageEn = "User is not a Patient, so forbid the action", MessageAr = "المستخدم ليس مريضًا، لذلك يتم منع الإجراء" });
            }

            try
            {
                // Update Patient entity
                patient.FirstName = patientDTO.FirstName;
                patient.LastName = patientDTO.LastName;
                patient.IsMale = patientDTO.IsMale;
                patient.Email = patientDTO.Email;
                patient.PhoneNumber = patientDTO.PhoneNumber;
                patient.Address = patientDTO.Address;
                patient.InsuranceCompany = patientDTO.InsuranceCompany;
                patient.BirthDate = patientDTO.BirthDate;
                patient.Governorate = patientDTO.Governorate;

                // Update User entity
                userPatient.Name = patientDTO.FirstName + "_" + patientDTO.LastName;
                userPatient.UserName = patientDTO.Email;
                userPatient.NormalizedUserName = patientDTO.Email.ToUpper();
                userPatient.Email = patientDTO.Email;
                userPatient.NormalizedEmail = patientDTO.Email.ToUpper();
                userPatient.PhoneNumber = patientDTO.PhoneNumber;

                _db.Update(patient);
                await _db.SaveChangesAsync();

                return Ok(new { StatusCode = 200, MessageEn = "Patient profile updated successfully.", MessageAr = "تم تحديث ملف المريض بنجاح." });
            }
            catch (DbUpdateException ex)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, new { StatusCode = 500, MessageEn = "An error occurred while updating the Patient profile.", MessageAr = "حدث خطأ أثناء تحديث ملف المريض." });
            }
        }
    }
}
