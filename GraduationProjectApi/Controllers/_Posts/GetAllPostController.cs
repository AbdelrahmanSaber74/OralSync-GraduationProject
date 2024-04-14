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


            string hosturl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";


                        var posts = query
                    .Where(m => m.IsVisible)
                   .Skip((page - 1) * PageSize)
                   .Take(PageSize)
                   .Join(
                       _db.Users, // Assuming dbContext is your EF Core DbContext
                       post => post.UserId,
                       user => user.Id,
                       (post, user) => new
                       {
                           post.PostId,
                           UserName = user.Name,
                           ProfileImage = hostUrl +  user.ProfileImage, // Assuming profile image is a property in the Users table
                           post.Title,
                           post.Content,
                           post.DateCreated,
                           post.DateUpdated,
                           post.TimeCreated,
                           post.TimeUpdated,
                           post.UserId,
                           LikeCount = post.Likes.Count,
                           PostImages = post.Image.Select(image => hostUrl + image).ToList(),
                           Comments = _db.Comments
                               .Where(comment => comment.PostId == post.PostId)
                               .Select(comment => new
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
                                   ProfileImage = _db.Users
                                       .Where(u => u.Id == comment.UserId)
                                       .Select(u => hostUrl + u.ProfileImage)
                                       .FirstOrDefault(),
                                   // Add other properties you need from the Comments table
                               })
                               .ToList()
                       })
                   .ToList();




            if (posts.Count > 0)
            {
                var result = new
                {
                    TotalPosts = totalPosts,
                    TotalPages = totalPages,
                    currentPage = page,
                    Posts = posts
                };
                return Ok(result);
            }

            return Ok(new { TotalPosts = totalPosts, TotalPages = totalPages, Posts = new object[0] });
        }
    }
}
