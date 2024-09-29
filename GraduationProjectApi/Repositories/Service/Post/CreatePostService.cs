using GraduationProjectApi.Repositories.IService.IPost;

namespace GraduationProjectApi.Repositories.IService.Post
{
    public class CreatePostService : ICreatePostService
    {
        private readonly ICreatePostService _postRepository;

        public CreatePostService(ICreatePostService postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<GraduationProjectApi.Models.Post> CreatePostAsync(string userId, string title, string content, IFormFileCollection fileCollection)
        {
            return await _postRepository.CreatePostAsync(userId, title, content, fileCollection);
        }
    }
}
