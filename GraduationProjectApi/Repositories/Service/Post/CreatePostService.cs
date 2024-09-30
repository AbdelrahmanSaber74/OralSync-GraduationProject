// GraduationProjectApi.Repositories.Service.Post.CreatePostService.cs
using GraduationProjectApi.Repositories.IService.IPost;
using GraduationProjectApi.Models;
using IdentityManagerServerApi.Data;
using Microsoft.AspNetCore.Http;

namespace GraduationProjectApi.Repositories.Service.Post
{
    public class CreatePostService : ICreatePostService
    {
        private readonly AppDbContext _db;

        public CreatePostService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<GraduationProjectApi.Models.Post> CreatePostAsync(string userId, string title, string content, IFormFileCollection fileCollection)
        {
            // Create a new Post entity
            var post = new GraduationProjectApi.Models.Post
            {
                UserId = userId,
                Title = title,
                Content = content,
                // Handle fileCollection to save files if needed
            };

            // Add the post to the DbSet
            await _db.Posts.AddAsync(post);

            // Save changes to the database
            await _db.SaveChangesAsync();

            return post;
        }
    }
}
