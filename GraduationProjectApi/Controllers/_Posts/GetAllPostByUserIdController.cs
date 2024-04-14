using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GraduationProjectApi.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using IdentityManagerServerApi.Data;

namespace GraduationProjectApi.Controllers.Posts
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GetAllPostByUserIdController : ControllerBase
    {
        private readonly AppDbContext _db;

        public GetAllPostByUserIdController(AppDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        [HttpGet]
        public async Task<IActionResult> Get(string userId = null)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound(new { StatusCode = 404, MessageEn = "User ID not found", MessageAr = "لم يتم العثور على معرف المستخدم" });
            }

            var hostUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            var posts = await _db.Posts
                .Where(m => m.IsVisible && m.UserId == userId)
                .Include(post => post.User)
                .OrderByDescending(p => p.PostId)
                .Select(post => new
                {
                    post.PostId,
                    UserName = post.User.Name,
                    ProfileImage = hostUrl + post.User.ProfileImage,
                    post.Title,
                    post.Content,
                    post.DateCreated,
                    post.DateUpdated,
                    post.TimeCreated,
                    post.TimeUpdated,
                    post.UserId,
                    LikeCount = post.Likes.Count,
                    PostImages = post.Image.Select(image => hostUrl + image).ToList(),
                    Comments = post.Comments.Select(comment => new
                    {
                        comment.CommentId,
                        comment.User.Name,
                        comment.Content,
                        comment.Title,
                        comment.DateCreated,
                        comment.TimeCreated,
                        comment.DateUpdated,
                        comment.TimeUpdated,
                        comment.UserId,
                        comment.PostId,
                        ProfileImage = hostUrl + comment.User.ProfileImage,
                    }).ToList()
                })
                .ToListAsync();

            return Ok(posts);
        }
    }
}
