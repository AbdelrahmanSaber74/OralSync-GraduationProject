using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GraduationProjectApi.Models;
using IdentityManagerServerApi.Data;
using IdentityManagerServerApi.Models;
using SharedClassLibrary.DTOs;
using SharedClassLibrary.Helper;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json.Serialization;
using System.Collections.Immutable;

namespace GraduationProjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddCommentController : ControllerBase
    {
        private readonly AppDbContext _db;

        public AddCommentController(AppDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
          
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddComment(CommentDTO commentDto)
        {
            try

            {

                var hostUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userName = User.FindFirst(ClaimTypes.Name)?.Value;
                var profileImage = _db.Users.Where(m => m.Id == userId).Select(m => m.ProfileImage).FirstOrDefault();

              

                if (string.IsNullOrEmpty(userId))
                    return StatusCode(StatusCodes.Status404NotFound, new { StatusCode = 404, MessageEn = "User ID not found", MessageAr = "لم يتم العثور على معرف المستخدم" });

                var post = _db.Posts.FirstOrDefault(m => m.PostId == commentDto.PostId);

                if (post == null)
                    return StatusCode(StatusCodes.Status404NotFound, new { StatusCode = 404, MessageEn = "Post not found", MessageAr = "لم يتم العثور على المنشور" });

                var comment = new Comment
                {
                    UserId = userId,
                    Name = userName,
                    PostId = commentDto.PostId,
                    Content = commentDto.Content,
                    Title = commentDto.Title,
                    DateCreated = DateTimeHelper.FormatDate(DateTime.Now),
                    TimeCreated = DateTimeHelper.FormatTime(DateTime.Now),
                    DateUpdated = "",
                    TimeUpdated = ""
                };


                _db.Comments.Add(comment);
                _db.SaveChanges();


                var notification = new Notification
                {
                    UserId = post.UserId,
                    SenderUserId = userId,
                    PostId = commentDto.PostId,
                    Type = NotificationType.Comment,
                    DateCreated = DateTimeHelper.FormatDate(DateTime.Now),
                    TimeCreated = DateTimeHelper.FormatTime(DateTime.Now),
                };

                _db.Notifications.Add(notification);
                _db.SaveChanges();

                var lastComment = _db.Comments
               .OrderByDescending(c => c.CommentId)
               .FirstOrDefault();



                if (lastComment != null)
                {
                    var response = new
                    {
                        lastComment.CommentId,
                        lastComment.Name,
                        lastComment.Content,
                        lastComment.Title,
                        lastComment.DateCreated,
                        lastComment.TimeCreated,
                        lastComment.DateUpdated,
                        lastComment.TimeUpdated,
                        lastComment.UserId,
                        lastComment.PostId,
                        ProfileImage = hostUrl + profileImage
                    };

                    return Ok(response);
                }



                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { StatusCode = 404, MessageEn = "No comments found", MessageAr = "لم يتم العثور على تعليقات" });
                }
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    StatusCode = 500,
                    MessageEn = $"Internal Server Error: {ex.Message}",
                    MessageAr = "حدث خطأ داخلي في الخادم",
                };
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }
    }
}
