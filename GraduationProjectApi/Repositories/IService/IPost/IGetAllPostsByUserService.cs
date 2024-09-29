namespace GraduationProjectApi.Repositories.IService.IPost
{
    public interface IGetAllPostsByUserService
    {
        Task<object> GetAllPostsByUserAsync(string userId, string hostUrl);
    }
}
