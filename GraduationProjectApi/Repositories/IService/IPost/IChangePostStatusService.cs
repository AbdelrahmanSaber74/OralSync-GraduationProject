using System.Collections.Generic;
using System.Threading.Tasks;
using GraduationProjectApi.Models;

namespace GraduationProjectApi.Repositories
{
    public interface IChangePostStatusService
    {
        Task<Post> GetPostByIdAndUserAsync(int postId, string userId);
        Task TogglePostVisibilityAsync(Post post);
    }
}
