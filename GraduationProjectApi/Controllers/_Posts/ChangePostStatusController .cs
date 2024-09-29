using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using GraduationProjectApi.Repositories.IService.IPost;

namespace GraduationProjectApi.Controllers._Posts
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, Doctor, Student")]
    public class ChangePostStatusController : ControllerBase
    {
        private readonly IChangePostStatusService _postRepository;

        public ChangePostStatusController(IChangePostStatusService postRepository)
        {
            _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
        }

        [HttpPut]
        public async Task<IActionResult> ChangeStatus(int postId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(StatusCodes.Status404NotFound, new { StatusCode = 404, MessageEn = "User ID not found", MessageAr = "لم يتم العثور على معرف المستخدم" });
            }

            var post = await _postRepository.GetPostByIdAndUserAsync(postId, userId);

            if (post == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { StatusCode = 404, MessageEn = "Post not found", MessageAr = "لم يتم العثور على المنشور" });
            }

            await _postRepository.TogglePostVisibilityAsync(post);

            string statusMessageEn = post.IsVisible ? "Post is now visible" : "Post is now hidden";
            string statusMessageAr = post.IsVisible ? "المنشور مرئي الآن" : "المنشور مخفي الآن";

            return StatusCode(StatusCodes.Status200OK, new { StatusCode = 200, MessageEn = statusMessageEn, MessageAr = statusMessageAr });
        }
    }
}
