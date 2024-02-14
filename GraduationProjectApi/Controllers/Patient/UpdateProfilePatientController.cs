using IdentityManagerServerApi.Data;
using IdentityManagerServerApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedClassLibrary.DTOs;
using System.Linq;
using System.Numerics;
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
                return StatusCode(StatusCodes.Status400BadRequest, new { StatusCode = 400, MessageEn = "User ID or Role not found in claims.", MessageAr = "لم يتم العثور على معرف المستخدم أو الدور في البيانات." });

            if (userRole == "Patient")
            {
                var patient = await _db.Patients.FirstOrDefaultAsync(x => x.UserId == userId);
                var userPatient = await _db.Users.FirstOrDefaultAsync(x => x.Id == userId);

                if (patient == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { StatusCode = 404, MessageEn = "Patient not found.", MessageAr = "لم يتم العثور على المريض." });
                }


                // update patient In table patient
                patient.FirstName = patientDTO.FirstName;
                patient.LastName = patientDTO.LastName;
                patient.IsMale = patientDTO.IsMale;
                patient.Email = patientDTO.Email;
                patient.PhoneNumber = patientDTO.PhoneNumber;
                patient.Address = patientDTO.Address;
                patient.InsuranceCompany = patientDTO.InsuranceCompany;
                patient.BirthDate = patientDTO.BirthDate;


                // update patient In table Users
                userPatient.Name = patientDTO.FirstName+"_"+patientDTO.LastName;
                userPatient.UserName = patientDTO.Email;
                userPatient.NormalizedUserName = patientDTO.Email.ToUpper();
                userPatient.Email = patientDTO.Email;
                userPatient.NormalizedEmail = patientDTO.Email.ToUpper();
                userPatient.PhoneNumber = patientDTO.PhoneNumber;

                _db.Update(patient);
                await _db.SaveChangesAsync();

                return StatusCode(StatusCodes.Status200OK, new { StatusCode = 200, MessageEn = "Patient profile updated successfully.", MessageAr = "تم تحديث ملف المريض بنجاح." });

            }


            return StatusCode(StatusCodes.Status403Forbidden, new { StatusCode = 403, MessageEn = "User is not a Patient, so forbid the action", MessageAr = "المستخدم ليس مريض ، لذلك يتم منع الإجراء" });

        }
    }
}
