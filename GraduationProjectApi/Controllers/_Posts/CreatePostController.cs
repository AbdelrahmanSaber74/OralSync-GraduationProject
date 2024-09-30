using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using GraduationProjectApi.Repositories.IService.IPost;

namespace GraduationProjectApi.Controllers._Posts
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, Doctor, Student")]
    public class CreatePostController : ControllerBase
    {
        private readonly ICreatePostService _postService;

        public CreatePostController(ICreatePostService postService)
        {
            _postService = postService ?? throw new ArgumentNullException(nameof(postService));
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(IFormFileCollection fileCollection, string title, string content)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                    return StatusCode(StatusCodes.Status404NotFound, new { StatusCode = 404, MessageEn = "User ID not found", MessageAr = "لم يتم العثور على معرف المستخدم" });

                var post = await _postService.CreatePostAsync(userId, title, content, fileCollection);

                return StatusCode(StatusCodes.Status200OK, new { StatusCode = 200, MessageEn = "Post created successfully", MessageAr = "تم إنشاء المنشور بنجاح" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { StatusCode = 500, MessageEn = $"An error occurred while saving the post. {ex}", MessageAr = $"حدث خطأ أثناء حفظ المنشور. {ex}" });
            }
        }
    }
}
