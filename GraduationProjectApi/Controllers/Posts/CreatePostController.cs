using GraduationProjectApi.Models;
using IdentityManagerServerApi.Data;
using IdentityManagerServerApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedClassLibrary.Helper;
using SharedClassLibrary.DTOs;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GraduationProjectApi.Controllers.Post
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Student,Doctor,Admin")]
    public class CreatePostController : ControllerBase
    {
        private readonly AppDbContext _db;

        public CreatePostController(AppDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(PostDto postDto)
        {
            try
            {
                // Get user id from claims
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                // Validate user id and role
                if (string.IsNullOrEmpty(userId))
                    return BadRequest(new { StatusCode = 400, Message = "User ID not found in claims." });

                // Create post entity
                var post = new GraduationProjectApi.Models.Post
                {
                    Title =postDto.Title,
                    Content = postDto.Content,
                    UserId = userId,
                    DateCreated = DateTimeHelper.FormatDate(DateTime.Now),
                    TimeCreated = DateTimeHelper.FormatTime(DateTime.Now),
                    DateUpdated = "",
                    TimeUpdated = "" ,
                };

                // Add post to database
                _db.Posts.Add(post);
                await _db.SaveChangesAsync();

                // Return successful response
                return Ok(new { StatusCode = 200, Message = "Post created successfully." });
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { StatusCode = 500, Message = $"An error occurred while saving the post. {ex}" });
            }
        }
    }
}
