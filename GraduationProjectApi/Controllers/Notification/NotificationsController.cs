using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GraduationProjectApi.Models;
using System;
using System.Linq;
using IdentityManagerServerApi.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Collections.Generic;

namespace GraduationProjectApi.Controllers._Posts
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public NotificationsController(AppDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }


        [HttpGet]
        public IActionResult GetNotifications()
        {
            // Get the user ID from the claims
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(StatusCodes.Status404NotFound, new { StatusCode = 404, MessageEn = "User ID not found", MessageAr = "لم يتم العثور على معرف المستخدم" });
            }

            try
            {
                // Query the database for notifications for the current user
                var notifications = _db.Notifications
                     .Where(n => n.UserId == userId)
                     .Join(
                         _db.Users,
                         notification => notification.SenderUserId,
                         user => user.Id,
                         (notification, user) => new
                         {
                             notification.NotificationId,
                             Sender = user.Name, 
                             notification.PostId,
                             Type = notification.Type.ToString(),
                             notification.IsRead,
                             notification.DateCreated,
                             notification.TimeCreated
                         })
                         .ToList();


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
