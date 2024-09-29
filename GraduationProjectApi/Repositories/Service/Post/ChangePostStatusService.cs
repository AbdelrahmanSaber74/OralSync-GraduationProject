using System.Linq;
using System.Threading.Tasks;
using GraduationProjectApi.Models;
using IdentityManagerServerApi.Data;
using Microsoft.EntityFrameworkCore;

namespace GraduationProjectApi.Repositories
{
    public class ChangePostStatusService : IChangePostStatusService
    {
        private readonly AppDbContext _db;

        public ChangePostStatusService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Post> GetPostByIdAndUserAsync(int postId, string userId)
        {
            return await _db.Posts.FirstOrDefaultAsync(p => p.PostId == postId && p.UserId == userId);
        }

        public async Task TogglePostVisibilityAsync(Post post)
        {
            post.IsVisible = !post.IsVisible;
            await _db.SaveChangesAsync();
        }
    }
}
