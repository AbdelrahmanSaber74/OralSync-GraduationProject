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

            // Get user id from claims
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Validate user id
            if (string.IsNullOrEmpty(userId))
                return StatusCode(StatusCodes.Status404NotFound, new { StatusCode = 404, MessageEn = "User ID not found", MessageAr = "لم يتم العثور على معرف المستخدم" });

            //Map the DTO to the Comment entity
            var comment = new Comment
           {
               UserId = userId,
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

            //Return a success response
            return StatusCode(StatusCodes.Status200OK, new { StatusCode = 200, MessageEn = "Comment added successfully", MessageAr = "تم إضافة التعليق بنجاح" });
        }
    }
}
