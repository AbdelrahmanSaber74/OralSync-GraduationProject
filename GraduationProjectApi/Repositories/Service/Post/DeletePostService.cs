using GraduationProjectApi.Models;
using IdentityManagerServerApi.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GraduationProjectApi.Services
{
    public class DeletePostService : IDeletePostService
    {
        private readonly AppDbContext _db;

        public DeletePostService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<bool> DeletePostAsync(int postId, string userId)
        {
            var post = await _db.Posts.FirstOrDefaultAsync(m => m.PostId == postId && m.UserId == userId);

            if (post == null)
            {
                return false; // Post not found or doesn't belong to the user
            }

            _db.Posts.Remove(post);
            await _db.SaveChangesAsync();

            return true; // Post deleted successfully
        }
    }
}
