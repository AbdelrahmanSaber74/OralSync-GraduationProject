using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GraduationProjectApi.Models;
using System;
using System.IO;
using System.Linq;
using IdentityManagerServerApi.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace GraduationProjectApi.Controllers._Posts
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GetAllPostByUserIdController : ControllerBase
    {
        private readonly AppDbContext _db;

        public GetAllPostByUserIdController (AppDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        [HttpGet]
        public IActionResult Get(string userId)
        {

            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(StatusCodes.Status404NotFound, new { StatusCode = 404, MessageEn = "User ID not found", MessageAr = "لم يتم العثور على معرف المستخدم" });
            }

            string hostUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            var posts = _db.Posts
                .Where(m => m.IsVisible && m.UserId == userId)
                .Include(post => post.User)
                .OrderByDescending(p => p.DateCreated)
                .Select(p => new
                {
                    p.PostId,
                    UserName = p.User.Name,
                    p.Title,
                    p.Content,
                    p.DateCreated,
                    p.DateUpdated,
                    p.TimeCreated,
                    p.TimeUpdated,
                    p.UserId,
                    p.Comments,
                    LikeCount = p.Likes.Count,
                    Image = p.Image.Select(image => hostUrl + image).ToList()
                })
                .ToList();

            if (posts.Count > 0)
            {
                return Ok(posts);
            }

            return Ok(new object[0]);
        }
    }
}
