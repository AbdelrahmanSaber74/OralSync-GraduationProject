using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GraduationProjectApi.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityManagerServerApi.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace GraduationProjectApi.Controllers._Posts
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, Doctor, Student")]
    public class DeletePostController : ControllerBase
    {
        private readonly AppDbContext _db;

        public DeletePostController(AppDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int postId)
        {
            // Get the current user's ID
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Check if user ID is not found
            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(StatusCodes.Status404NotFound, new { StatusCode = 404, MessageEn = "User ID not found", MessageAr = "لم يتم العثور على معرف المستخدم" });
            }

            // Get the post to delete
            var post = await _db.Posts.FirstOrDefaultAsync(m => m.PostId == postId && m.UserId == userId);

            // If post is not found or doesn't belong to the user, return 404
            if (post == null)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { StatusCode = 403, MessageEn = "Post not found or doesn't belong to the user", MessageAr = "المنشور غير موجود أو لا ينتمي إلى المستخدم" });

            }

            // Remove the post
            _db.Posts.Remove(post);
            await _db.SaveChangesAsync();

            // Return success message

            return StatusCode(StatusCodes.Status200OK, new { StatusCode = 200, MessageEn = "Post deleted successfully", MessageAr = "تم حذف المنشور بنجاح" });
        }
    }
}
