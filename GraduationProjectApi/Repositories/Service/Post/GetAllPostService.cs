using GraduationProjectApi.Repositories.IService.IPost;
using IdentityManagerServerApi.Data;
using Microsoft.EntityFrameworkCore;

namespace GraduationProjectApi.Repositories.IService.Post
{
    public class GetAllPostService : IGetAllPostService
    {
        private readonly AppDbContext _db;
        private const int PageSize = 10;

        public GetAllPostService(AppDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<(object posts, int totalPosts, int totalPages)> GetAllPostsAsync(int page, string hostUrl)
        {
            var query = _db.Posts
                .Where(m => m.IsVisible)
                .Include(post => post.User)
                .OrderByDescending(p => p.PostId);

            var totalPosts = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalPosts / PageSize);

            var posts = await query
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .Select(post => new
                {
                    post.PostId,
                    UserName = post.User.Name,
                    ProfileImage = hostUrl + post.User.ProfileImage,
                    post.Title,
                    post.Content,
                    post.DateCreated,
                    post.DateUpdated,
                    post.UserId,
                    LikeCount = post.Likes.Count,
                    PostImages = post.Image.Select(image => hostUrl + image).ToList(),
                    Comments = post.Comments.Select(comment => new
                    {
                        comment.CommentId,
                        UserName = comment.User.Name,
                        comment.Content,
                        comment.Title,
                        comment.DateCreated,
                        comment.UserId,
                        comment.PostId,
                        ProfileImage = hostUrl + comment.User.ProfileImage
                    }).ToList()
                })
                .ToListAsync();

            return (posts, totalPosts, totalPages);
        }
    }
}
