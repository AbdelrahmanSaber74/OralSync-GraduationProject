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
                return StatusCode(StatusCodes.Status400BadRequest, new { StatusCode = 400, MessageEn = "User ID or Role not found in claims.", MessageAr = "لم يتم العثور على معرف المستخدم أو الدور في البيانات." });

            if (userRole == "Doctor")
            {
                var doctor = await _db.Doctors.FirstOrDefaultAsync(x => x.UserId == userId);
                var userDoctor = await _db.Users.FirstOrDefaultAsync(x => x.Id == userId);

                if (doctor == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { StatusCode = 404, MessageEn = "Doctor not found.", MessageAr = "الطبيب غير موجود." });
                }

                // update Doctor In table Doctor
                doctor.FirstName = doctorDTO.FirstName;
                doctor.LastName = doctorDTO.LastName;
                doctor.IsMale = doctorDTO.IsMale;
                doctor.PhoneNumber = doctorDTO.PhoneNumber;
                doctor.Email = doctorDTO.Email;
                doctor.UniversityName = doctorDTO.UniversityName;
                doctor.GPA = doctorDTO.GPA;
                doctor.ClinicAddress = doctorDTO.ClinicAddress;
                doctor.ClinicNumber = doctorDTO.ClinicNumber;
                doctor.InsuranceCompanies = doctorDTO.InsuranceCompanies;
                doctor.Certificates = doctorDTO.Certificates;
                doctor.GraduationDate = doctorDTO.GraduationDate;
                doctor.BirthDate = doctorDTO.BirthDate;



                // update Doctor In table Users
                userDoctor.Name = doctorDTO.FirstName+"_"+doctorDTO.LastName;
                userDoctor.UserName = doctorDTO.Email;
                userDoctor.NormalizedUserName = doctorDTO.Email.ToUpper();
                userDoctor.Email = doctorDTO.Email;
                userDoctor.NormalizedEmail = doctorDTO.Email.ToUpper();
                userDoctor.PhoneNumber = doctorDTO.PhoneNumber;



                _db.Update(doctor);
                await _db.SaveChangesAsync();

                return StatusCode(StatusCodes.Status200OK, new { StatusCode = 200, MessageEn = "Doctor profile updated successfully.", MessageAr = "تم تحديث ملف الطبيب بنجاح." });
            }


            return StatusCode(StatusCodes.Status403Forbidden, new { StatusCode = 403, MessageEn = "User is not a Doctor, so forbid the action", MessageAr = "المستخدم ليس طبيب ، لذلك يتم منع الإجراء" });

        }
    }
}
