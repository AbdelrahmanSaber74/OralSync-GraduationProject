using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraduationProjectApi.Services
{
    public interface IGetAllHiddenPostsByUserService
    {
        Task<IEnumerable<object>> GetHiddenPostsByUserAsync(string userId, string hostUrl);
    }
}
