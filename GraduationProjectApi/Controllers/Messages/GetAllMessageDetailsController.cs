using GraduationProjectApi.Models;
using IdentityManagerServerApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;

namespace GraduationProjectApi.Controllers.Messages
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GetAllMessageDetailsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public GetAllMessageDetailsController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // Get the user ID from the claims
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            string hosturl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";


                     var allMessages = await _db.Messages
                    .Where(m => m.SenderId == userId || m.ReceiverId == userId)
                    .Join(_db.Users,
                    message => message.SenderId,
                    sender => sender.Id,
                    (message, sender) => new { message, sender })
                    .Join(_db.Users,
                    temp => temp.message.ReceiverId,
                    receiver => receiver.Id,
                    (temp, receiver) => new
                    {
                    MessageId = temp.message.Id,
                    Sender = new
                    {
                      SenderId = temp.sender.Id,
                      SenderName = temp.sender.Name,
                      SenderProfileImage = hosturl + temp.sender.ProfileImage
                    },
                    Receiver = new
                    {
                      ReceiverId = receiver.Id,
                      ReceiverName = receiver.Name,
                      ReceiverProfileImage = hosturl + receiver.ProfileImage
                    }
                    })
                    .ToListAsync();

                var uniqueMessages = allMessages
                    .GroupBy(x => new { x.Sender.SenderId, x.Receiver.ReceiverId })
                    .Select(g => g.First())
                    .ToList();




            // Assuming you want to return all messages
            return Ok(uniqueMessages);
        }
    }
}
