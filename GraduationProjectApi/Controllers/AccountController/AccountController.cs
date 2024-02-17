using IdentityManagerServerApi.Data;
using IdentityManagerServerApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedClassLibrary.Contracts;
using SharedClassLibrary.DTOs;
using SharedClassLibrary.Helper;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
namespace IdentityManagerServerApi.Controllers.AccountController
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {


        private readonly IUserAccount _userAccount;
        private readonly AppDbContext _db;

        public AccountController(IUserAccount userAccount , AppDbContext db)
        {
            _userAccount = userAccount;
            _db = db;

        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDTO userDTO)
        {
            var response = await _userAccount.CreateAccount(userDTO);

            return Ok(response);

        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            AppDbContext _db = new AppDbContext();

            var response = await _userAccount.LoginAccount(loginDTO);

            if (response == null || string.IsNullOrEmpty(response.Token))
            {
                return BadRequest(new { StatusCode = 400, Message = "Invalid Email Or Password" });
            }

            var (userId, userRole) = GetUserIdAndRoleFromToken(response.Token); // Get UserId And userRole Using Token

            if (userId == null || userRole == null)
            {
                return BadRequest(new { StatusCode = 400, Message = "Error processing token" });
            }

            var userDetails = await getUserInformation(userId, userRole); // Get userDetails Using userId and userRole

            if (userDetails == null)
            {
                return BadRequest(new { StatusCode = 400, Message = "Error retrieving user details" });
            }


            return Ok(new
            {
                response.Token,
                userRole,
                userDetails
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


            IQueryable<object> resultQuery = null;

            switch (userRole)
            {
                case "Doctor":
                    resultQuery = _db.Doctors.Where(m => m.UserId == userId).Select(m => new { m.FirstName, m.LastName, m.IsMale, m.PhoneNumber, m.Email, m.UniversityName, m.GPA, m.ClinicAddress, m.ClinicNumber, m.InsuranceCompanies, m.Certificates, m.GraduationDate, m.BirthDate });
                    break;
                case "Student":
                    resultQuery = _db.Students.Where(m => m.UserId == userId).Select(m => new { m.FirstName, m.LastName, m.IsMale, m.PhoneNumber, m.Email, m.UniversityName, m.UniversitAddress, m.GPA, m.AcademicYear, m.BirthDate });
                    break;
                case "Patient":
                    resultQuery = _db.Patients.Where(m => m.UserId == userId).Select(m => new { m.FirstName, m.LastName, m.IsMale, m.Email, m.PhoneNumber, m.Address, m.InsuranceCompany, m.BirthDate });
                    break;
                case "Admin":
                    resultQuery = _db.Users.Where(m => m.Id == userId).Select(m => new { m.Id, m.Name });
                    break;
                default: break;
            }

            if (resultQuery == null)
            {
                return BadRequest(new { StatusCode = 400, Message = "Error retrieving resultQuery" });

            }

            return await resultQuery.FirstOrDefaultAsync(); // Await here



        }




    }
}
