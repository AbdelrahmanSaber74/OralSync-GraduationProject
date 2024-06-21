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
                .Join(_db.Users,
                      message => message.SenderId,
                      sender => sender.Id,
                      (message, sender) => new { message, sender })
                .Join(_db.Users,
                      temp => temp.message.ReceiverId,
                      receiver => receiver.Id,
                      (temp, receiver) => new
                      {
                          temp.message.Id,
                          Sender = new
                          {
                              Id = temp.sender.Id,
                              ProfileImage = hosturl + temp.sender.ProfileImage
                          },
                          Receiver = new
                          {
                              Id = receiver.Id,
                              ProfileImage = hosturl + receiver.ProfileImage
                          }
                      })
                .ToListAsync();


            // Assuming you want to return all messages
            return Ok(allMessages);
        }
    }
}
