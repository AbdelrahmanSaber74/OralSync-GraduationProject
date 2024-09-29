using GraduationProjectApi.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraduationProjectApi.Services
{
    public interface ICreatePostService
    {
        Task<Post> CreatePostAsync(string userId, string title, string content, IFormFileCollection fileCollection);
    }
}
