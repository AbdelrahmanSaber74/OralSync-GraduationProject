using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GraduationProjectApi.Models;
using System;
using System.Linq;
using IdentityManagerServerApi.Data;
using IdentityManagerServerApi.Models;
using SharedClassLibrary.DTOs;
using System.Security.Claims;
using SharedClassLibrary.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Hosting;

namespace GraduationProjectApi.Controllers.Likes
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddLikeController : ControllerBase
    {
        private readonly AppDbContext _db;

        public AddLikeController(AppDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        [HttpPost]
        [Authorize]
        public IActionResult Post(int PostId)
        {
            try
            {
                // Get user id from claims
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                // Validate user id
                if (string.IsNullOrEmpty(userId))
                    return StatusCode(StatusCodes.Status404NotFound, new { StatusCode = 404, MessageEn = "User ID not found", MessageAr = "لم يتم العثور على معرف المستخدم" });

                // Check if the user has already liked the post
                var existingLike = _db.Likes.FirstOrDefault(l => l.UserId == userId && l.PostId == PostId);

                if (existingLike != null)
                {
                    // If the user has already liked the post, remove the like (convert to dislike)
                    _db.Likes.Remove(existingLike);
                    _db.SaveChanges();

                    // Return a success response indicating that the like has been converted to a dislike
                    return StatusCode(StatusCodes.Status200OK, new { StatusCode = 200, MessageEn = "Like converted to dislike successfully", MessageAr = "تم تحويل الإعجاب إلى عدم إعجاب بنجاح" });
                }

                // Map the DTO to the Like entity
                var like = new Like
                {
                    UserId = userId,
                    PostId = PostId,
                    DateCreated = DateTimeHelper.FormatDate(DateTime.Now),
                    TimeCreated = DateTimeHelper.FormatTime(DateTime.Now),
                    DateUpdated = "",
                    TimeUpdated = ""
                };

                // Add the like to the database
                _db.Likes.Add(like);
                _db.SaveChanges();

                // Create a notification for the post owner
                var post = _db.Posts.FirstOrDefault(m => m.PostId == PostId);
                if (post != null)
                {
                    var notification = new Notification
                    {
                        UserId = post.UserId,
                        SenderUserId = userId,
                        PostId = PostId,
                        Type = NotificationType.Like,
                        DateCreated = DateTimeHelper.FormatDate(DateTime.Now),
                        TimeCreated = DateTimeHelper.FormatTime(DateTime.Now)
                    };

                    _db.Notifications.Add(notification);
                    _db.SaveChanges();
                }

                // Return a success response
                return StatusCode(StatusCodes.Status200OK, new { StatusCode = 200, MessageEn = "Like added successfully", MessageAr = "تمت إضافة الإعجاب بنجاح" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { StatusCode = 500, MessageEn = "An error occurred while processing the request", MessageAr = "حدث خطأ أثناء معالجة الطلب" });
            }
        }
    }
}


