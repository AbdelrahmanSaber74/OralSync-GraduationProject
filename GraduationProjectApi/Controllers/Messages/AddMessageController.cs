using GraduationProjectApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GraduationProjectApi.Controllers.Messages
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddMessageController : ControllerBase
    {
        private readonly IAddMessageService _messageService;

        public AddMessageController(IAddMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpPost]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> Post(string receiverId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return StatusCode(StatusCodes.Status404NotFound, new { StatusCode = 404, MessageEn = "User ID not found", MessageAr = "لم يتم العثور على معرف المستخدم" });

            return await _messageService.SendMessageAsync(receiverId, userId);
        }
    }
}
