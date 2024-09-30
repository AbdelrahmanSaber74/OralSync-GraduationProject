using GraduationProjectApi.Models;
using GraduationProjectApi.Repositories; // Add this using directive
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GraduationProjectApi.Controllers.Messages
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GetAllMessageDetailsController : ControllerBase
    {
        private readonly IGetAllMessageDetailsService _messageService;

        public GetAllMessageDetailsController(IGetAllMessageDetailsService messageRepository)
        {
            _messageService = messageRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var messages = await _messageService.GetAllMessagesAsync(userId);

            var uniqueMessages = messages
                .GroupBy(x => new { x.SenderId, x.ReceiverId })
                .Select(g => g.First())
                .ToList();

            return Ok(uniqueMessages);
        }
    }
}
