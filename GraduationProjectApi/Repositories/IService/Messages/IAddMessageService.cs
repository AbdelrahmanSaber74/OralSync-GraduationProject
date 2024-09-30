using Microsoft.AspNetCore.Mvc;

namespace GraduationProjectApi.Services
{
    public interface IAddMessageService
    {
        Task<IActionResult> SendMessageAsync(string receiverId, string senderId);
    }
}
