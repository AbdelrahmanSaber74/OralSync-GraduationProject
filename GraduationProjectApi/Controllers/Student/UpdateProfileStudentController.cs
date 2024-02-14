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
    public class UpdateProfileStudentController : ControllerBase
    {
        private readonly AppDbContext _db;

        public UpdateProfileStudentController(AppDbContext db)
        {
            _db = db;
        }

        [HttpPut]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Put(StudentDTO studentDTO)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new { StatusCode = 400, MessageEn = "User ID not found in claims.", MessageAr = "معرف المستخدم غير موجود في البيانات." });
            }

            var student = await _db.Students.FirstOrDefaultAsync(x => x.UserId == userId);
            var userstudent = await _db.Users.FirstOrDefaultAsync(x => x.Id == userId); ;

            if (student == null)
            {
                return NotFound(new { StatusCode = 404, MessageEn = "Student not found.", MessageAr = "الطالب غير موجود." });
            }

            if (!User.IsInRole("Student"))
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { StatusCode = 403, MessageEn = "User is not a student, so forbid the action", MessageAr = "المستخدم ليس طالبًا، لذلك يتم منع الإجراء" });
            }

            try
            {
                // Update Student entity
                student.FirstName = studentDTO.FirstName;
                student.LastName = studentDTO.LastName;
                student.IsMale = studentDTO.IsMale;
                student.PhoneNumber = studentDTO.PhoneNumber;
                student.Email = studentDTO.Email;
                student.UniversityName = studentDTO.UniversityName;
                student.UniversitAddress = studentDTO.UniversityAddress;
                student.GPA = studentDTO.GPA;
                student.AcademicYear = studentDTO.AcademicYear;
                student.BirthDate = studentDTO.BirthDate;

                // Update User entity
                userstudent.Name = studentDTO.FirstName + "_" + studentDTO.LastName;
                userstudent.UserName = studentDTO.Email;
                userstudent.NormalizedUserName = studentDTO.Email.ToUpper();
                userstudent.Email = studentDTO.Email;
                userstudent.NormalizedEmail = studentDTO.Email.ToUpper();
                userstudent.PhoneNumber = studentDTO.PhoneNumber;


                _db.Update(student);
                await _db.SaveChangesAsync();

                return Ok(new { StatusCode = 200, MessageEn = "Student profile updated successfully.", MessageAr = "تم تحديث ملف الطالب بنجاح." });
            }
            catch (DbUpdateException ex)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, new { StatusCode = 500, MessageEn = "An error occurred while updating the student profile.", MessageAr = "حدث خطأ أثناء تحديث ملف الطالب." });
            }
        }
    }
}
