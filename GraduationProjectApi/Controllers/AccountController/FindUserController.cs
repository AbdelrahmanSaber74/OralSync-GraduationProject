using IdentityManagerServerApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedClassLibrary.Contracts;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityManagerServerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] 
    public class FindUserController : ControllerBase
    {
        private readonly AppDbContext _db;

        public FindUserController(AppDbContext db )
        {
            _db = db;

        }

        [HttpGet]
        public async Task<IActionResult> GetUserRoles()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userRole))
            return BadRequest(new { StatusCode = 400, Message = "User ID or Role not found in claims." });


            IQueryable<object> resultQuery = null;

            switch (userRole)
            {
                case "Doctor":
                    resultQuery = _db.Doctors.Where(m => m.UserId == userId).Select(m => new {m.FirstName , m.LastName  , m.IsMale , m.UserId});
                    break;
                case "Student":
                    resultQuery = _db.Students.Where(m => m.UserId == userId).Select(m => new { m.FirstName, m.LastName, m.IsMale, m.UserId }); 
                    break;
                case "Patient":
                    resultQuery = _db.Patients.Where(m => m.UserId == userId).Select(m => new { m.FirstName, m.LastName, m.IsMale, m.UserId }); 
                    break;
                case "Admin":
                    resultQuery = _db.Users.Where(m => m.Id == userId).Select(m => new { m.Id , m.Name }); 
                    break;
                default:
                    return BadRequest(new { StatusCode = 400, Message = "Invalid user role." });
            }



            var result = await resultQuery.ToListAsync();

            if (result.Any())
            {
                return Ok(new { UserRole = userRole, userInformation = result });
            }

            else
            {
                return NotFound(new { StatusCode = 404, Message = "NotFound" });

            }

        }
    }
}
