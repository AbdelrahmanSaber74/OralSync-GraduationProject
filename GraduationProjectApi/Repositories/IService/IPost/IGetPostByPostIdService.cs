namespace GraduationProjectApi.Repositories.IService.IPost
{
    public interface IGetPostByPostIdService
    {
        Task<object> GetPostByIdAsync(int postId, string hostUrl);
    }
}
