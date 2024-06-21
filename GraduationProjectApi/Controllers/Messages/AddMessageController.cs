using GraduationProjectApi.Models;
using IdentityManagerServerApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedClassLibrary.Helper;
using System.Security.Claims;

namespace GraduationProjectApi.Controllers.Messages
{
    [Route("api/[controller]")]
    [ApiController]

    public class AddMessageController : ControllerBase
    {
        private readonly AppDbContext _db;

        public AddMessageController(AppDbContext db)
        {
            _db = db;
        }


        [HttpPost]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> Post(string ReceiverId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            Message message = new Message()
            {
                SenderId = userId,
                ReceiverId = ReceiverId,
                DateCreated = DateTimeHelper.FormatDate(DateTime.Now),
                TimeCreated = DateTimeHelper.FormatTime(DateTime.Now),
            };

            _db.Messages.Add(message);
            await _db.SaveChangesAsync();

            return Ok(new
            {
                StatusCode = StatusCodes.Status200OK,
                MessageEn = "Record saved successfully",
                MessageAr = "تم الحفظ بنجاح"
            });

        }


    }
}
