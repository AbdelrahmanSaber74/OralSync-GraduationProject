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
    public class UpdateProfileStudentController : ControllerBase
    {
        private readonly AppDbContext _db;

        public UpdateProfileStudentController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Get(StudentDTO studentDTO)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userRole))
                return StatusCode(StatusCodes.Status400BadRequest, new { StatusCode = 400, MessageEn = "User ID or Role not found in claims.", MessageAr = "لم يتم العثور على معرف المستخدم أو الدور في البيانات." });

            if (userRole == "Student")
            {
                var student = await _db.Students.FirstOrDefaultAsync(x => x.UserId == userId);
                var userstudent = await _db.Users.FirstOrDefaultAsync(x => x.Id == userId);

                if (student == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { StatusCode = 404, MessageEn = "Student not found.", MessageAr = "الطالب غير موجود." });
                }


                // update student In table student
                student.FirstName = studentDTO.FirstName;
                student.LastName = studentDTO.LastName;
                student.IsMale = studentDTO.IsMale;
                student.PhoneNumber = studentDTO.PhoneNumber;
                student.Email = studentDTO.Email;
                student.UniversityName = studentDTO.UniversityName;
                student.UniversitAddress = studentDTO.UniversitAddress;
                student.GPA = studentDTO.GPA;
                student.AcademicYear = studentDTO.AcademicYear;
                student.BirthDate = studentDTO.BirthDate;



                // update student In table Users
                userstudent.Name = studentDTO.FirstName + "_" + studentDTO.LastName;
                userstudent.UserName = studentDTO.Email;
                userstudent.NormalizedUserName = studentDTO.Email.ToUpper();
                userstudent.Email = studentDTO.Email;
                userstudent.NormalizedEmail = studentDTO.Email.ToUpper();
                userstudent.PhoneNumber = studentDTO.PhoneNumber;


                _db.Update(student);
                await _db.SaveChangesAsync();

                return StatusCode(StatusCodes.Status200OK, new { StatusCode = 200, MessageEn = "Student profile updated successfully.", MessageAr = "تم تحديث ملف الطالب بنجاح." });
            }

            return StatusCode(StatusCodes.Status403Forbidden, new { StatusCode = 403, MessageEn = "User is not a student, so forbid the action", MessageAr = "المستخدم ليس طالبًا، لذلك يتم منع الإجراء" });


        }
    }
}
