namespace GraduationProjectApi.Repositories.IService.IPost
{
    public interface IGetAllPostByUserIdService
    {
        Task<IEnumerable<object>> GetPostsByUserIdAsync(string userId, string hostUrl);
    }
}
