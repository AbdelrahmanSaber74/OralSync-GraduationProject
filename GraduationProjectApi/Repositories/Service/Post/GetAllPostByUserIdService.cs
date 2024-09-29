using GraduationProjectApi.Models;
using IdentityManagerServerApi.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProjectApi.Services
{
    public class GetAllPostByUserIdService : IGetAllPostByUserIdService
    {
        private readonly AppDbContext _db;

        public GetAllPostByUserIdService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<object>> GetPostsByUserIdAsync(string userId, string hostUrl)
        {
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

            return posts;
        }
    }
}
