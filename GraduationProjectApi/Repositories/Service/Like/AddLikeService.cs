using GraduationProjectApi.Models;
using IdentityManagerServerApi.Data;
using IdentityManagerServerApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedClassLibrary.Helper;

namespace GraduationProjectApi.Services
{
    public class AddLikeService : IAddLikeService
    {
        private readonly AppDbContext _db;

        public AddLikeService(AppDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<IActionResult> AddOrRemoveLikeAsync(int postId, string userId)
        {
            try
            {
                // Check if the user has already liked the post
                var existingLike = await _db.Likes.FirstOrDefaultAsync(l => l.UserId == userId && l.PostId == postId);

                if (existingLike != null)
                {
                    // If the user has already liked the post, remove the like (convert to dislike)
                    _db.Likes.Remove(existingLike);
                    await _db.SaveChangesAsync();

                    return new OkObjectResult(new { StatusCode = 200, MessageEn = "Like converted to dislike successfully", MessageAr = "تم تحويل الإعجاب إلى عدم إعجاب بنجاح" });
                }

                // Map the DTO to the Like entity
                var like = new Like
                {
                    UserId = userId,
                    PostId = postId,
                    DateCreated = DateTimeHelper.FormatDate(DateTime.Now),
                    TimeCreated = DateTimeHelper.FormatTime(DateTime.Now),
                    DateUpdated = "",
                    TimeUpdated = ""
                };

                // Add the like to the database
                await _db.Likes.AddAsync(like);
                await _db.SaveChangesAsync();

                // Create a notification for the post owner
                var post = await _db.Posts.FirstOrDefaultAsync(m => m.PostId == postId);
                if (post != null)
                {
                    var notification = new Notification
                    {
                        UserId = post.UserId,
                        SenderUserId = userId,
                        PostId = postId,
                        Type = NotificationType.Like,
                        DateCreated = DateTimeHelper.FormatDate(DateTime.Now),
                        TimeCreated = DateTimeHelper.FormatTime(DateTime.Now)
                    };

                    await _db.Notifications.AddAsync(notification);
                    await _db.SaveChangesAsync();
                }

                return new OkObjectResult(new { StatusCode = 200, MessageEn = "Like added successfully", MessageAr = "تمت إضافة الإعجاب بنجاح" });
            }
            catch (Exception)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
