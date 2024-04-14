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
using System.Text.Json.Serialization;
using System.Text.Json;

namespace GraduationProjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddCommentController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly JsonSerializerOptions _jsonOptions;

        public AddCommentController(AppDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _jsonOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };
        }


        [HttpPost]
        [Authorize]
        public IActionResult AddComment(CommentDTO commentDto)
        {

            // Get user id from claims
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            // Get user id from claims
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;

            // Validate user id
            if (string.IsNullOrEmpty(userId))
                return StatusCode(StatusCodes.Status404NotFound, new { StatusCode = 404, MessageEn = "User ID not found", MessageAr = "لم يتم العثور على معرف المستخدم" });


            var post = _db.Posts.Where(m => m.PostId == commentDto.PostId).FirstOrDefault();


            // Check if the post exists
            if (post == null)
            {
                // Return a not found response
                return StatusCode(StatusCodes.Status404NotFound, new { StatusCode = 404, MessageEn = "Post not found", MessageAr = "لم يتم العثور على المنشور" });
            }


            //Map the DTO to the Comment entity
            var comment = new Comment
           {
               UserId = userId,
               Name = userName,
               PostId = commentDto.PostId ,
               Content = commentDto.Content,
               Title = commentDto.Title,
                DateCreated = DateTimeHelper.FormatDate(DateTime.Now),
                TimeCreated = DateTimeHelper.FormatTime(DateTime.Now),
                DateUpdated = "",
                TimeUpdated = ""
            };

            //Add the comment to the database
            _db.Comments.Add(comment);
            _db.SaveChanges();




            // Create a notification for the post owner
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


            var responseData = new
            {
                CommentId = comment.CommentId,
                Name = comment.Name,
                Content = comment.Content,
                Title = comment.Title,
                DateCreated = comment.DateCreated,
                TimeCreated = comment.TimeCreated,
                DateUpdated = comment.DateUpdated,
                TimeUpdated = comment.TimeUpdated,
                UserId = comment.UserId,
                PostId = comment.PostId
            };


            // Serialize the anonymous object and return it
            return Ok(JsonSerializer.Serialize(responseData, _jsonOptions));




            //Return a success response
            //return StatusCode(StatusCodes.Status200OK, new { StatusCode = 200, MessageEn = "Comment added successfully", MessageAr = "تم إضافة التعليق بنجاح" });
        }
    }
}
