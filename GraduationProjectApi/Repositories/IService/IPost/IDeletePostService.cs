using System.Threading.Tasks;

namespace GraduationProjectApi.Services
{
    public interface IDeletePostService
    {
        Task<bool> DeletePostAsync(int postId, string userId);
    }
}
