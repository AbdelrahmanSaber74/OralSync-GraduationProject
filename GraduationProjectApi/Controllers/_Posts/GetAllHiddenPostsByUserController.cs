using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using GraduationProjectApi.Services;

namespace GraduationProjectApi.Controllers.Posts
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GetAllHiddenPostsByUserController : ControllerBase
    {
        private readonly IGetAllHiddenPostsByUserService _hiddenPostsService;

        public GetAllHiddenPostsByUserController(IGetAllHiddenPostsByUserService hiddenPostsService)
        {
            _hiddenPostsService = hiddenPostsService ?? throw new ArgumentNullException(nameof(hiddenPostsService));
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

            var posts = await _hiddenPostsService.GetHiddenPostsByUserAsync(userId, hostUrl);

            return Ok(posts);
        }
    }
}
