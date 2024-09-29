using GraduationProjectApi.Models;
using GraduationProjectApi.Repositories;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace GraduationProjectApi.Services
{
    public class CreatePostService : ICreatePostService
    {
        private readonly ICreatePostService _postRepository;

        public CreatePostService(ICreatePostService postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<Post> CreatePostAsync(string userId, string title, string content, IFormFileCollection fileCollection)
        {
            return await _postRepository.CreatePostAsync(userId, title, content, fileCollection);
        }
    }
}
