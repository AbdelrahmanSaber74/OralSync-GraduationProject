using GraduationProjectApi.Repositories.IService.IPost;
using IdentityManagerServerApi.Data;
using Microsoft.EntityFrameworkCore;

namespace GraduationProjectApi.Repositories.IService.Post
{
    public class ChangePostStatusService : IChangePostStatusService
    {
        private readonly AppDbContext _db;

        public ChangePostStatusService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<GraduationProjectApi.Models.Post> GetPostByIdAndUserAsync(int postId, string userId)
        {
            return await _db.Posts.FirstOrDefaultAsync(p => p.PostId == postId && p.UserId == userId);
        }

        public async Task TogglePostVisibilityAsync(GraduationProjectApi.Models.Post post)
        {
            post.IsVisible = !post.IsVisible;
            await _db.SaveChangesAsync();
        }

       
    }
}
