using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GraduationProjectApi.Repositories.IService.IPost;

namespace GraduationProjectApi.Controllers.Posts
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GetPostByPostIdController : ControllerBase
    {
        private readonly IGetPostByPostIdService _postService;

        public GetPostByPostIdController(IGetPostByPostIdService postService)
        {
            _postService = postService ?? throw new ArgumentNullException(nameof(postService));
        }

        [HttpGet]
        public async Task<IActionResult> Get(int postId)
        {
            string hostUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            var post = await _postService.GetPostByIdAsync(postId, hostUrl);

            if (post != null)
            {
                return Ok(post);
            }

            return NotFound(new { StatusCode = 404, MessageEn = "Post ID not found", MessageAr = "لم يتم العثور على المنشور" });
        }
    }
}
