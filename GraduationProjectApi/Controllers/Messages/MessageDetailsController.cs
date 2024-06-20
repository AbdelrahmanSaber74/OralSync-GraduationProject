using GraduationProjectApi.Models;
using IdentityManagerServerApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GraduationProjectApi.Controllers.Messages
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MessageDetailsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public MessageDetailsController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string DoctorId)
        {
            // Get the user ID from the claims
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            string hosturl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";


            var senderUser = await _db.Users
                .Where(m => m.Id == userId)
                .Select(m => new { 
                    m.Id,
                    m.Name,
                    ProfileImage= hosturl + m.ProfileImage,
                    m.IsActive,
                   
                })
                .FirstOrDefaultAsync();

            var receiverUser = await _db.Users
                .Where(m => m.Id == DoctorId)
                .Select(m => new {
                    m.Id,
                    m.Name,
                    ProfileImage = hosturl + m.ProfileImage,
                    m.IsActive,

                })
                .FirstOrDefaultAsync();

            if (senderUser == null || receiverUser == null)
            {
                return NotFound("One or both users not found.");
            }

            return Ok(new
            {
                SenderUser = senderUser,
                ReceiverUser = receiverUser
            });
        }
    }
}
