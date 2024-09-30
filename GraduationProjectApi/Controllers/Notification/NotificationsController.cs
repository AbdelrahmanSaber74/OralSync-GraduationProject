using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GraduationProjectApi.Models;
using GraduationProjectApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace GraduationProjectApi.Controllers._Posts
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationservice;

        public NotificationsController(INotificationService notificationservice)
        {
            notificationservice = _notificationservice;
        }

        [HttpGet]
        public async Task<IActionResult> GetNotifications()
        {
            // Get the user ID from the claims
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(StatusCodes.Status404NotFound, new { StatusCode = 404, MessageEn = "User ID not found", MessageAr = "لم يتم العثور على معرف المستخدم" });
            }

            try
            {
                // Fetch notifications using the repository
                var notifications = await _notificationservice.GetUserNotificationsAsync(userId);

                return Ok(notifications);
            }
            catch (Exception ex)
            {
                // Log the error
                return StatusCode(StatusCodes.Status500InternalServerError, new { StatusCode = 500, MessageEn = "An error occurred while fetching notifications", MessageAr = "حدث خطأ أثناء جلب الإشعارات" });
            }
        }
    }
}
