using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GraduationProjectApi.Services;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace GraduationProjectApi.Controllers.Posts
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GetAllPostsByUserController : ControllerBase
    {
        private readonly IGetAllPostsByUserService _postService;

        public GetAllPostsByUserController(IGetAllPostsByUserService postService)
        {
            _postService = postService ?? throw new ArgumentNullException(nameof(postService));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return NotFound(new { StatusCode = 404, MessageEn = "User ID not found", MessageAr = "لم يتم العثور على معرف المستخدم" });
            }

            var hostUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            var posts = await _postService.GetAllPostsByUserAsync(userId, hostUrl);

            return Ok(posts);
        }
    }
}
