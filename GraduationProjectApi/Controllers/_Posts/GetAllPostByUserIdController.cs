using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GraduationProjectApi.Repositories.IService.IPost;

namespace GraduationProjectApi.Controllers.Posts
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GetAllPostByUserIdController : ControllerBase
    {
        private readonly IGetAllPostByUserIdService _postService;

        public GetAllPostByUserIdController(IGetAllPostByUserIdService postService)
        {
            _postService = postService ?? throw new ArgumentNullException(nameof(postService));
        }

        [HttpGet]
        public async Task<IActionResult> Get(string userId = null)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound(new { StatusCode = 404, MessageEn = "User ID not found", MessageAr = "لم يتم العثور على معرف المستخدم" });
            }

            var hostUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            var posts = await _postService.GetPostsByUserIdAsync(userId, hostUrl);

            return Ok(posts);
        }
    }
}
