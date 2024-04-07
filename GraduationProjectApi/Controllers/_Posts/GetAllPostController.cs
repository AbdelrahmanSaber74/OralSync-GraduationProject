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
    public class GetAllPostController : ControllerBase
    {
        private readonly AppDbContext _db;
        private const int PageSize = 10;

        public GetAllPostController(AppDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        [HttpGet]
        public IActionResult Get(int page = 1)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(StatusCodes.Status404NotFound, new { StatusCode = 404, MessageEn = "User ID not found", MessageAr = "لم يتم العثور على معرف المستخدم" });
            }

            string hostUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            var query = _db.Posts
                .Where(m => m.IsVisible)
                .Include(post => post.User)
                .OrderByDescending(p => p.DateCreated);

            var totalPosts = query.Count();
            var totalPages = (int)Math.Ceiling((double)totalPosts / PageSize);

            var posts = query
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
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
                var result = new
                {
                    TotalPosts = totalPosts,
                    TotalPages = totalPages,
                    Posts = posts
                };
                return Ok(result);
            }

            return Ok(new { TotalPosts = totalPosts, TotalPages = totalPages, Posts = new object[0] });
        }
    }
}
