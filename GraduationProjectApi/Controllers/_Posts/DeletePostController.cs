using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using GraduationProjectApi.Services;

namespace GraduationProjectApi.Controllers._Posts
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, Doctor, Student")]
    public class DeletePostController : ControllerBase
    {
        private readonly IDeletePostService _deletePostService;

        public DeletePostController(IDeletePostService deletePostService)
        {
            _deletePostService = deletePostService ?? throw new ArgumentNullException(nameof(deletePostService));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int postId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(StatusCodes.Status404NotFound, new { StatusCode = 404, MessageEn = "User ID not found", MessageAr = "لم يتم العثور على معرف المستخدم" });
            }

            bool isDeleted = await _deletePostService.DeletePostAsync(postId, userId);

            if (!isDeleted)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { StatusCode = 403, MessageEn = "Post not found or doesn't belong to the user", MessageAr = "المنشور غير موجود أو لا ينتمي إلى المستخدم" });
            }

            return StatusCode(StatusCodes.Status200OK, new { StatusCode = 200, MessageEn = "Post deleted successfully", MessageAr = "تم حذف المنشور بنجاح" });
        }
    }
}
