namespace GraduationProjectApi.Repositories.IService.IPost
{
    public interface IGetAllHiddenPostsByUserService
    {
        Task<IEnumerable<object>> GetHiddenPostsByUserAsync(string userId, string hostUrl);
    }
}
