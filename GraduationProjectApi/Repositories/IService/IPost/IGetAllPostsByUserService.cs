using System.Threading.Tasks;

namespace GraduationProjectApi.Services
{
    public interface IGetAllPostsByUserService
    {
        Task<object> GetAllPostsByUserAsync(string userId, string hostUrl);
    }
}
