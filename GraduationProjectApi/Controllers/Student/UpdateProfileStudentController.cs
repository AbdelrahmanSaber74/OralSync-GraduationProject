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

        [HttpGet]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Get(StudentDTO studentDTO)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userRole))
                return BadRequest(new { StatusCode = 400, Message = "User ID or Role not found in claims." });

            if (userRole == "Student")
            {
                var student = await _db.Students.FirstOrDefaultAsync(x => x.UserId == userId);

                if (student == null)
                {
                    return NotFound(new { StatusCode = 404, Message = "Student not found." });
                }

                // Example update to GPA
                student.GPA = studentDTO.GPA;
                student.PhoneNumber = studentDTO.PhoneNumber;
               

                _db.Update(student);
                await _db.SaveChangesAsync();

                return Ok(new { StatusCode = 200, Message = "Student profile updated successfully." });
            }


            return Forbid(); // User is not a student, so forbid the action.

        }
    }
}
