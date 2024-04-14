using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GraduationProjectApi.Models;
using System;
using System.Linq;
using IdentityManagerServerApi.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks; // Add this namespace for Task

namespace GraduationProjectApi.Controllers.Posts
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GetPostByPostIdController : ControllerBase
    {
        private readonly AppDbContext _db;

        public GetPostByPostIdController(AppDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        [HttpGet]
        public async Task<IActionResult> Get(int postId)
        {
            string hostUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            var post = await _db.Posts
                .Where(m => m.PostId == postId && m.IsVisible)
                .Include(post => post.User)
                 .OrderByDescending(p => p.PostId)
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
                    Comments = p.Comments.Select(comment => new
                    {
                        comment.CommentId,
                        UserName = comment.User.Name,
                        comment.Content,
                        comment.Title,
                        comment.DateCreated,
                        comment.DateUpdated,
                        comment.TimeCreated,
                        comment.TimeUpdated,
                        comment.UserId,
                        comment.PostId,
                        ProfileImage = hostUrl + comment.User.ProfileImage,
                    }).ToList(),
                    LikeCount = p.Likes.Count,
                    Image = p.Image.Select(image => hostUrl + image).ToList()
                })
                .FirstOrDefaultAsync();

            if (post != null)
            {
                return Ok(post);
            }

            return NotFound(new { StatusCode = 404, MessageEn = "Post ID not found", MessageAr = "لم يتم العثور على المنشور" });
        }
    }
}
