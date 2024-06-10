//using GraduationProjectApi.Models;
//using IdentityManagerServerApi.Data;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.SignalR;
//using Microsoft.EntityFrameworkCore;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace GraduationProjectApi.Controllers.Chat
//{
//    [ApiController]
//    [Route("[controller]")]
//    public class MessagesController : ControllerBase
//    {
//        private readonly AppDbContext _context;

//        public MessagesController(AppDbContext context)
//        {
//            _context = context;
//        }

//        [HttpGet("{senderId}/{receiverId}")]
//        public async Task<ActionResult<IEnumerable<Message>>> GetMessages(string senderId, string receiverId)
//        {
//            var messages = await _context.Messages
//                .Where(m => (m.SenderId == senderId && m.ReceiverId == receiverId) ||
//                            (m.SenderId == receiverId && m.ReceiverId == senderId))
//                .OrderBy(m => m.TimeCreated)
//                .ToListAsync();

//            if (messages == null)
//            {
//                return NotFound();
//            }

//            return Ok(messages);
//        }

//        [HttpPost]
//        public async Task<ActionResult<Message>> SendMessage(Message message)
//        {
//            _context.Messages.Add(message);
//            await _context.SaveChangesAsync();

//            return Ok(message);
//        }
//    }

//    public class ChatHub : Hub
//    {
//        private readonly AppDbContext _context;

//        public ChatHub(AppDbContext context)
//        {
//            _context = context;
//        }

//        public async Task SendMessage(string senderId, string receiverId, string message)
//        {
//            var newMessage = new GraduationProjectApi.Models.Message
//            {
//                SenderId = senderId,
//                ReceiverId = receiverId,
//                Content = message,
//            };

//            _context.Messages.Add(newMessage);
//            await _context.SaveChangesAsync();

//            await Clients.User(receiverId).SendAsync("ReceiveMessage", newMessage);
//        }

//        public async Task<IEnumerable<GraduationProjectApi.Models.Message>> GetMessages(string senderId, string receiverId)
//        {
//            return await _context.Messages
//                .Where(m => (m.SenderId == senderId && m.ReceiverId == receiverId) ||
//                            (m.SenderId == receiverId && m.ReceiverId == senderId))
//                .OrderBy(m => m.TimeCreated)
//                .ToListAsync();
//        }
//    }
//}
