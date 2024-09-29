using GraduationProjectApi.Repositories.IService.IPost;
using IdentityManagerServerApi.Data;
using Microsoft.EntityFrameworkCore;

namespace GraduationProjectApi.Repositories.IService.Post
{
    public class GetPostByPostIdService : IGetPostByPostIdService
    {
        private readonly AppDbContext _db;

        public GetPostByPostIdService(AppDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<object> GetPostByIdAsync(int postId, string hostUrl)
        {
            if (postId <= 0)
            {
                throw new ArgumentException("Post ID must be greater than zero.", nameof(postId));
            }

            var post = await _db.Posts
                .Where(m => m.PostId == postId && m.IsVisible)
                .Include(p => p.User)
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

            return post;
        }
    }
}
