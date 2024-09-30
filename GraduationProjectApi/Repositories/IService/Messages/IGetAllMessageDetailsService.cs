using GraduationProjectApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraduationProjectApi.Repositories
{
    public interface  IGetAllMessageDetailsService
    {
        Task<IEnumerable<Message>> GetAllMessagesAsync(string userId);
    }

}
