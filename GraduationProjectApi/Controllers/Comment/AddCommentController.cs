using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GraduationProjectApi.Models;
using SharedClassLibrary.DTOs;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using GraduationProjectApi.Repositories.IService;

namespace GraduationProjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddCommentController : ControllerBase
    {
        private readonly IAddCommentService _addCommentService;

        public AddCommentController(IAddCommentService addCommentService)
        {
            _addCommentService = addCommentService ?? throw new ArgumentNullException(nameof(addCommentService));
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddComment(CommentDTO commentDto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userName = User.FindFirst(ClaimTypes.Name)?.Value;

                if (string.IsNullOrEmpty(userId))
                    return NotFound(new { StatusCode = 404, MessageEn = "User ID not found", MessageAr = "لم يتم العثور على معرف المستخدم" });

                var post = _addCommentService.GetPostById(commentDto.PostId);
                if (post == null)
                    return NotFound(new { StatusCode = 404, MessageEn = "Post not found", MessageAr = "لم يتم العثور على المنشور" });

                var comment = _addCommentService.AddComment(commentDto, userId, userName);

                var hostUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
                var profileImage = _addCommentService.GetUserProfileImage(userId); // Use the service method

                var response = new
                {
                    comment.CommentId,
                    comment.Name,
                    comment.Content,
                    comment.Title,
                    comment.DateCreated,
                    comment.TimeCreated,
                    ProfileImage = string.IsNullOrEmpty(profileImage) ? null : hostUrl + profileImage
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    StatusCode = 500,
                    MessageEn = $"Internal Server Error: {ex.Message}",
                    MessageAr = "حدث خطأ داخلي في الخادم",
                };
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }
    }
}
