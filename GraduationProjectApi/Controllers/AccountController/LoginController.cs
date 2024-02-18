using IdentityManagerServerApi.Data;
using IdentityManagerServerApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedClassLibrary.Contracts;
using SharedClassLibrary.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityManagerServerApi.Controllers.AccountController
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserAccount _userAccount;
        private readonly AppDbContext _db;

        public LoginController(IUserAccount userAccount, AppDbContext db)
        {
            _userAccount = userAccount;
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var response = await _userAccount.LoginAccount(loginDTO);

            if (response == null || string.IsNullOrEmpty(response.Token))
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { StatusCode = 400, MessageEn = "Invalid Email Or Password", MessageAr = "البريد الإلكتروني أو كلمة المرور غير صالحة" });
            }

            var (userId, userRole) = GetUserIdAndRoleFromToken(response.Token);

            if (userId == null || userRole == null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { StatusCode = 401, MessageEn = "Error processing token", MessageAr = "خطأ في معالجة الرمز المميز" });
            }

            var userDetails = await getUserInformation(userId, userRole);

            if (userDetails == null)
            {
                return StatusCode(StatusCodes.Status402PaymentRequired, new { StatusCode = 402, MessageEn = "Error retrieving user details", MessageAr = "خطأ في استرداد تفاصيل المستخدم" });
            }


            string hosturl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            var profileImage = _db.Users.Where(m=>m.Id == userId).Select(m=>m.ProfileImage).FirstOrDefault();

            return Ok(new
            {
                response.Token,
                userRole,
                userDetails,
                profileImage = hosturl + profileImage
            });
        }

        private (string userId, string userRole) GetUserIdAndRoleFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var decodedToken = handler.ReadJwtToken(token);

            var userId = decodedToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            var userRole = decodedToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role)?.Value;

            return (userId, userRole);
        }

        private async Task<object> getUserInformation(string userId, string userRole)
        {

            string hosturl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";

            IQueryable<object> resultQuery = null;

            switch (userRole)
            {
                case "Doctor":
                    resultQuery = _db.Doctors.Where(m => m.UserId == userId).Select(m => new { m.FirstName, m.LastName, m.IsMale, m.PhoneNumber, m.Email, m.UniversityName, m.GPA, m.ClinicAddress, m.ClinicNumber, m.InsuranceCompanies, m.Certificates, m.GraduationDate, m.BirthDate  });
                    break;
                case "Student":
                    resultQuery = _db.Students.Where(m => m.UserId == userId).Select(m => new { m.FirstName, m.LastName, m.IsMale, m.PhoneNumber, m.Email, m.UniversityName, m.UniversitAddress, m.GPA, m.AcademicYear, m.BirthDate });
                    break;
                case "Patient":
                    resultQuery = _db.Patients.Where(m => m.UserId == userId).Select(m => new { m.FirstName, m.LastName, m.IsMale, m.Email, m.PhoneNumber, m.Address, m.InsuranceCompany, m.BirthDate});
                    break;
                case "Admin":
                    resultQuery = _db.Users.Where(m => m.Id == userId).Select(m => new { m.Id, m.Name });
                    break;
                default: break;
            }

            if (resultQuery == null)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { StatusCode = 403, MessageEn = "Error retrieving resultQuery", MessageAr = "خطأ في استرجاع الاستعلام" });

            }

            return await resultQuery.FirstOrDefaultAsync(); // Await here



        }




    }
}
