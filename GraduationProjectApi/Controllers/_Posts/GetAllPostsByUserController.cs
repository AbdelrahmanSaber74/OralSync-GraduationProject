using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GraduationProjectApi.Models;
using System.Collections.Generic;
using IdentityManagerServerApi.Data;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace GraduationProjectApi.Controllers._Posts
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, Doctor, Student")]
    public class GetAllPostsByUserController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly AppDbContext _db;

        public GetAllPostsByUserController(IWebHostEnvironment environment, AppDbContext db)
        {
            _environment = environment;
            _db = db;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var posts = _db.Posts
                            .Where(m => m.UserId == userId && m.IsVisible)
                            .ToList();

            if (posts.Any())
            {
                string hostUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

                var result = posts.Select(p => new
                {
                    p.PostId,
                    p.Title,
                    p.Content,
                    p.DateCreated,
                    p.DateUpdated,
                    p.TimeCreated,
                    p.TimeUpdated,
                    p.UserId,
                    p.Comments,
                    p.Likes,
                    Image = p.Image.Select(image => hostUrl + image).ToList() 
                    }).ToList();

                return Ok(result);
            }

            return StatusCode(StatusCodes.Status404NotFound, new { StatusCode = 404, MessageEn = "No posts found for the provided user ID", MessageAr = "لم يتم العثور على مشاركات لمعرف المستخدم" });
        }


        private string GetFilePath(string userId, string postId)
        {
            return Path.Combine(_environment.WebRootPath, $"Post\\{userId}\\{postId}");
        }


    }
}
