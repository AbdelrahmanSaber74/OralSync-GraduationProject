using System.Threading.Tasks;

namespace GraduationProjectApi.Services
{
    public interface IGetAllPostService
    {
        Task<(object posts, int totalPosts, int totalPages)> GetAllPostsAsync(int page, string hostUrl);
    }
}
