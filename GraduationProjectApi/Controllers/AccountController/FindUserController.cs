﻿using IdentityManagerServerApi.Data;
using IdentityManagerServerApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedClassLibrary.Contracts;
using SharedClassLibrary.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GraduationProjectApi.Controllers.AccountController
{
    [Route("api/[controller]")]
    [ApiController]
    public class FindUserController : ControllerBase
    {
        private readonly IUserAccount _userAccount;
        private readonly AppDbContext _db;

        public FindUserController(IUserAccount userAccount, AppDbContext db)
        {
            _userAccount = userAccount;
            _db = db;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;


            var userDetails = await getUserInformation(userId, userRole);


            if (userDetails == null)
            {
                return StatusCode(StatusCodes.Status402PaymentRequired, new { StatusCode = 402, MessageEn = "Error retrieving user details", MessageAr = "خطأ في استرداد تفاصيل المستخدم" });
            }


            var isActive = _db.Users
             .Where(m => m.Id == userId && m.IsActive == false)
             .FirstOrDefault();

            if (isActive != null)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable, new { StatusCode = 406, MessageEn = "Your account is not active. Please contact support for assistance or consider substituting it.", MessageAr = "حسابك غير نشط. يرجى الاتصال بالدعم للحصول على المساعدة أو النظر في استبداله." });
            }

            string hosturl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            var profileImage = _db.Users.Where(m => m.Id == userId).Select(m => m.ProfileImage).FirstOrDefault();

            return Ok(new
            {
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
                    resultQuery = _db.Doctors.Where(m => m.UserId == userId).Select(m => new { m.FirstName, m.LastName, m.IsMale, m.PhoneNumber, m.Email, m.UniversityName, m.GPA, m.ClinicAddresses, m.ClinicNumber, m.InsuranceCompanies, m.Certificates, m.GraduationDate, m.BirthDate , m.Governorate });
                    break;
                case "Student":
                    resultQuery = _db.Students.Where(m => m.UserId == userId).Select(m => new { m.FirstName, m.LastName, m.IsMale, m.PhoneNumber, m.Email, m.UniversityName, m.UniversitAddress, m.GPA, m.AcademicYear, m.BirthDate, m.Governorate });
                    break;
                case "Patient":
                    resultQuery = _db.Patients.Where(m => m.UserId == userId).Select(m => new { m.FirstName, m.LastName, m.IsMale, m.Email, m.PhoneNumber, m.Address, m.InsuranceCompany, m.BirthDate, m.Governorate });
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
