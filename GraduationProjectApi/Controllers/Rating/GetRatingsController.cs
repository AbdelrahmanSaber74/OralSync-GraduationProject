using IdentityManagerServerApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProjectApi.Controllers.Rating
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetRatingsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public GetRatingsController(AppDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get(string userId)
        {
            try
            {
                // Retrieve ratings with necessary properties
                var ratedUserRatings = await _db.Ratings
                    .Where(m => m.RatedUserId == userId)
                    .Select(m => new
                    {
                        m.RatedUserId,
                        m.SenderUserId,
                        m.Value,
                        m.Comment,
                        m.DateCreated,
                        m.TimeCreated
                    })
                    .ToListAsync();

                // Check if no ratings found
                if (!ratedUserRatings.Any())
                    return StatusCode(StatusCodes.Status404NotFound, new { StatusCode = 404, MessageEn = "User ID not found", MessageAr = "لم يتم العثور على معرف المستخدم" });

                return Ok(ratedUserRatings);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                // logger.LogError(ex, "An error occurred while processing the request");

                // Return error response
                return StatusCode(StatusCodes.Status500InternalServerError, new { StatusCode = 500, MessageEn = "An error occurred while processing the request", MessageAr = "حدث خطأ أثناء معالجة الطلب" });

            }
        }
    }
}
