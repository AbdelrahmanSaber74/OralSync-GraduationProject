using IdentityManagerServerApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GraduationProjectApi.Controllers.AccountController
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeleteAcoountController : ControllerBase
    {
        private readonly AppDbContext _db;

        public DeleteAcoountController(AppDbContext db)
        {
            _db = db;
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            // Get the user ID from the claims
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Load the user account with related entities (posts, comments, likes)
            var user = _db.Users
                .Include(u => u.Posts)
                    .ThenInclude(p => p.Comments)
                .Include(u => u.Likes)
                .FirstOrDefault(m => m.Id == userId);

            if (user == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { StatusCode = 404, MessageEn = "User not found.", MessageAr = "المستخدم غير موجود." });
            }

            // Delete related entities (comments)
            foreach (var post in user.Posts)
            {
                _db.Comments.RemoveRange(post.Comments);
            }

            // Delete related entities (likes)
            _db.Likes.RemoveRange(user.Likes);

            // Delete related entities (posts)
            _db.Posts.RemoveRange(user.Posts);

            // Delete the user account
            _db.Users.Remove(user);

            // Save changes to the database
            await _db.SaveChangesAsync();

            return StatusCode(StatusCodes.Status200OK, new { StatusCode = 200, MessageEn = "User deleted successfully.", MessageAr = "تم حذف المستخدم بنجاح." });
        }
    }
}
