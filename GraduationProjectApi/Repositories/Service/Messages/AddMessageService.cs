using GraduationProjectApi.Models;
using IdentityManagerServerApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedClassLibrary.Helper;
using System;
using System.Threading.Tasks;

namespace GraduationProjectApi.Services
{
    public class AddMessageService : IAddMessageService
    {
        private readonly AppDbContext _db;

        public AddMessageService(AppDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<IActionResult> SendMessageAsync(string receiverId, string senderId)
        {
            var message = new Message
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                DateCreated = DateTimeHelper.FormatDate(DateTime.Now),
                TimeCreated = DateTimeHelper.FormatTime(DateTime.Now),
            };

            _db.Messages.Add(message);
            await _db.SaveChangesAsync();

            return new OkObjectResult(new
            {
                StatusCode = StatusCodes.Status200OK,
                MessageEn = "Record saved successfully",
                MessageAr = "تم الحفظ بنجاح"
            });
        }
    }
}
