using GraduationProjectApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GraduationProjectApi.Controllers._Posts
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GetAllPostController : ControllerBase
    {
        private readonly IGetAllPostService _getAllPostService;

        public GetAllPostController(IGetAllPostService getAllPostService)
        {
            _getAllPostService = getAllPostService ?? throw new ArgumentNullException(nameof(getAllPostService));
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(StatusCodes.Status404NotFound, new { StatusCode = 404, MessageEn = "User ID not found", MessageAr = "لم يتم العثور على معرف المستخدم" });
            }

            // Generate the host URL
            var hostUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            // Call the service method
            var (posts, totalPosts, totalPages) = await _getAllPostService.GetAllPostsAsync(page, hostUrl);

            var result = new
            {
                TotalPosts = totalPosts,
                TotalPages = totalPages,
                CurrentPage = page,
                Posts = posts
            };

            return Ok(result);
        }
    }
}
