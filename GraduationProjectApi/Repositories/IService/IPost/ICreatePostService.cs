using GraduationProjectApi.Models;

namespace GraduationProjectApi.Repositories.IService.IPost
{
    public interface ICreatePostService
    {
        Task<GraduationProjectApi.Models.Post> CreatePostAsync(string userId, string title, string content, IFormFileCollection fileCollection);
    }
}
