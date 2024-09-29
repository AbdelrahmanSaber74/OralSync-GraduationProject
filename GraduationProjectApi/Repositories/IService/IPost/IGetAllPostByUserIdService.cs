using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraduationProjectApi.Services
{
    public interface IGetAllPostByUserIdService
    {
        Task<IEnumerable<object>> GetPostsByUserIdAsync(string userId, string hostUrl);
    }
}
