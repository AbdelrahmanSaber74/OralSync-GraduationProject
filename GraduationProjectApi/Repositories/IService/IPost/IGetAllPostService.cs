namespace GraduationProjectApi.Repositories.IService.IPost
{
    public interface IGetAllPostService
    {
        Task<(object posts, int totalPosts, int totalPages)> GetAllPostsAsync(int page, string hostUrl);
    }
}
