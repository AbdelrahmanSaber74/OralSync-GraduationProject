using System.Threading.Tasks;

namespace GraduationProjectApi.Services
{
    public interface IGetPostByPostIdService
    {
        Task<object> GetPostByIdAsync(int postId, string hostUrl);
    }
}
