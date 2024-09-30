using GraduationProjectApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GraduationProjectApi.Controllers.Likes
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddLikeController : ControllerBase
    {
        private readonly IAddLikeService _likeService;

        public AddLikeController(IAddLikeService likeService)
        {
            _likeService = likeService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post(int postId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Validate user id
            if (string.IsNullOrEmpty(userId))
                return StatusCode(StatusCodes.Status404NotFound, new { StatusCode = 404, MessageEn = "User ID not found", MessageAr = "لم يتم العثور على معرف المستخدم" });

            return await _likeService.AddOrRemoveLikeAsync(postId, userId);
        }
    }
}
