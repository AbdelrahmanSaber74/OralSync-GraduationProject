using IdentityManagerServerApi.Data;
using Microsoft.AspNetCore.Mvc;
using SharedClassLibrary.Helper;
using SharedClassLibrary.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace GraduationProjectApi.Controllers.Rating
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddRateController : ControllerBase
    {
        private readonly AppDbContext _db;

        public AddRateController(AppDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }


        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Put([FromBody] RatingDTO Rating)
        {
            try
            {

                var SenderUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                // Check if the sender user exists
                if (SenderUserId == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { StatusCode = 404, MessageEn = "Rated user not found", MessageAr = "لم يتم العثور على معرف المستخدم" });
                }
                

                var ratedUser = _db.Users.FirstOrDefault(u => u.Id == Rating.RatedUserId);

                // Check if the rated user exists
                if (ratedUser == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { StatusCode = 404, MessageEn = "Rated user not found", MessageAr = "لم يتم العثور على معرف المستخدم" });
                }


                // Check if the rating already exists
                var existingRating = _db.Ratings
                    .FirstOrDefault(r => r.SenderUserId == SenderUserId && r.RatedUserId == Rating.RatedUserId);

                if (existingRating != null)
                {
                    return StatusCode(StatusCodes.Status409Conflict, new { StatusCode = 409, MessageEn = "Rating already exists", MessageAr = "التقييم موجود بالفعل" });
                }


                // Create a new Rating entity
                var newRating = new IdentityManagerServerApi.Models.Rating
                {
                    RatedUserId = Rating.RatedUserId,
                    SenderUserId = SenderUserId,
                    Value = Rating.Value,
                    Comment = Rating.Comment,
                    DateCreated = DateTimeHelper.FormatDate(DateTime.Now),
                    TimeCreated = DateTimeHelper.FormatTime(DateTime.Now)
                };



                // Add the new rating to the database
                _db.Add(newRating);
                await _db.SaveChangesAsync();

                return StatusCode(StatusCodes.Status200OK, new { StatusCode = 200, MessageEn = "Rating added successfully", MessageAr = "تم إضافة التقييم بنجاح" });



            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { StatusCode = 500, MessageEn = $"An error occurred while processing the request{ex.InnerException}", MessageAr = "حدث خطأ أثناء معالجة الطلب" });
            }


        }
    }
}
