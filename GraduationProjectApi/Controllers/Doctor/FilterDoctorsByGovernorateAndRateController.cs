using GraduationProjectApi.Models;
using IdentityManagerServerApi.Data;
using IdentityManagerServerApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedClassLibrary.Contracts;
using SharedClassLibrary.DTOs;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProjectApi.Controllers.Doctor
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilterDoctorsByGovernorateAndRateController : ControllerBase
    {
        private readonly IUserAccount _userAccount;
        private readonly AppDbContext _db;

        public FilterDoctorsByGovernorateAndRateController(IUserAccount userAccount, AppDbContext db)
        {
            _userAccount = userAccount ?? throw new ArgumentNullException(nameof(userAccount));
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }
        [HttpPost]
        public async Task<IActionResult> Get(string governorate = null, double minRate = 0)
        {
            try
            {
                IQueryable<GraduationProjectApi.Models.Doctor> doctorsQuery = _db.Doctors;

                if (!string.IsNullOrEmpty(governorate))
                {
                    doctorsQuery = doctorsQuery.Where(d => d.Governorate == governorate);
                }

                var doctorsList = await doctorsQuery.ToListAsync();
                var doctorsWithRate = new List<Models.Doctor>();

                foreach (var doctor in doctorsList)
                {
                    var averageRate = await CalculateAverageRate(doctor.UserId); // Assuming UserId is the property representing the user ID
                    if (averageRate >= minRate)
                    {
                        doctorsWithRate.Add(doctor);
                    }
                }

                return Ok(doctorsWithRate);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                return StatusCode(StatusCodes.Status500InternalServerError, new { StatusCode = 500, MessageEn = "An error occurred while filtering doctors by governorate and rate.", MessageAr = "حدث خطأ أثناء تصفية الأطباء حسب المحافظة والتقييم." });
            }
        }



        private async Task<double> CalculateAverageRate(string userId)
        {
            var ratedUserRatings = await _db.Ratings
                .Where(r => r.RatedUserId == userId)
                .Select(r => r.Value)
                .ToListAsync();

            return ratedUserRatings.Any() ? Math.Round(ratedUserRatings.Average(), 2) : 0;
        }
    }
}
