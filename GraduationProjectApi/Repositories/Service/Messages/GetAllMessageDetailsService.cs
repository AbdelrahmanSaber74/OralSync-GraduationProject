using GraduationProjectApi.Models;
using IdentityManagerServerApi.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProjectApi.Repositories
{
    public class GetAllMessageDetailsService : IGetAllMessageDetailsService
    {
        private readonly AppDbContext _db;

        public GetAllMessageDetailsService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Message>> GetAllMessagesAsync(string userId)
        {
            var allMessages = await _db.Messages
                .Where(m => m.SenderId == userId || m.ReceiverId == userId)
                .ToListAsync();

            return allMessages;
        }
    }
}
