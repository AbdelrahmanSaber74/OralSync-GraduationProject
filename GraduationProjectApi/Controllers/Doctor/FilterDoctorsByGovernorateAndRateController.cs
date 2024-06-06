using GraduationProjectApi.Models;
using IdentityManagerServerApi.Data;
using IdentityManagerServerApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedClassLibrary.Contracts;
using System;
using System.Collections.Generic;
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
        [Authorize]
        public async Task<IActionResult> Get(string governorate = null, double minRate = 0)
        {
            try
            {
                string hostUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";

                var doctorsQuery = _db.Doctors
                    .Include(d => d.User)
                    .Where(d => string.IsNullOrEmpty(governorate) || d.Governorate == governorate)
                    .Select(d => new
                    {
                        Doctor = d,
                        d.User.ProfileImage,
                        d.User.Name,
                        Ratings = _db.Ratings.Where(r => r.RatedUserId == d.UserId).Select(r => r.Value)
                    });

                var doctorsList = await doctorsQuery.ToListAsync();

                var doctorsWithRate = doctorsList
                    .Select(d => new
                    {
                        Doctor = d.Doctor,
                        ProfileImage = hostUrl + d.ProfileImage,
                        d.Name,
                        AverageRate = d.Ratings.Any() ? Math.Round(d.Ratings.Average(), 2) : 0
                    })
                    .Where(d => d.AverageRate >= minRate)
                    .ToList();

                return Ok(doctorsWithRate);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = 500,
                    MessageEn = $"An error occurred while filtering doctors by governorate and rate. {ex.Message}",
                    MessageAr = "حدث خطأ أثناء تصفية الأطباء حسب المحافظة والتقييم."
                });
            }
        }
    }
}
