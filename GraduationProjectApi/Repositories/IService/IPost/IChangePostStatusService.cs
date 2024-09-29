namespace GraduationProjectApi.Repositories.IService.IPost
{
    public interface IChangePostStatusService
    {
        Task<GraduationProjectApi.Models.Post> GetPostByIdAndUserAsync(int postId, string userId);
        Task TogglePostVisibilityAsync(GraduationProjectApi.Models.Post post);
    }
}
