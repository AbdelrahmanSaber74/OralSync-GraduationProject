namespace GraduationProjectApi.Repositories.IService.IPost
{
    public interface IDeletePostService
    {
        Task<bool> DeletePostAsync(int postId, string userId);
    }
}
