using GraduationProjectApi.Models;
using IdentityManagerServerApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedClassLibrary.Helper;

namespace GraduationProjectApi.Controllers.Messages
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class MessageController : ControllerBase
    {
        private readonly AppDbContext _db;

        public MessageController(AppDbContext db)
        {
            _db = db;
        }



        [HttpPost("send")]
        public async Task<IActionResult> SendMessage(string SenderId, string ReceiverId, string Content)
        {
            Message message = new Message()
            {
                Content = Content,
                SenderId = SenderId,
                ReceiverId = ReceiverId,
                DateCreated = DateTimeHelper.FormatDate(DateTime.Now),
                TimeCreated = DateTimeHelper.FormatTime(DateTime.Now),
            };

            _db.Messages.Add(message);
            await _db.SaveChangesAsync();

            return Ok(new {StatusCode = StatusCodes.Status200OK,MessageEn = "Message sent successfully",MessageAr = "تم إرسال الرسالة بنجاح" });


        }
    



        [HttpGet("receive")]
        public async Task<IActionResult> ReceiveMessages(string SenderId, string ReceiverId, int page = 1)
        {

            int pageSize = 10;

            var query = _db.Messages
                .Where(m => (m.SenderId == SenderId && m.ReceiverId == ReceiverId) ||
                            (m.SenderId == ReceiverId && m.ReceiverId == SenderId))
                .OrderByDescending(m => m.TimeCreated);

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var messages = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var response = new
            {
                TotalCount = totalCount,
                TotalPages = totalPages,
                CurrentPage = page,
                PageSize = pageSize,
                Messages = messages
            };

            return Ok(response);
        }





    }
}
