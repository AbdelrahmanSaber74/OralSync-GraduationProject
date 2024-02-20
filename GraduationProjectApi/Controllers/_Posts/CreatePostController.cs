using GraduationProjectApi.Models;
using IdentityManagerServerApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedClassLibrary.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GraduationProjectApi.Controllers._Posts
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CreatePostController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly AppDbContext _db;

        public CreatePostController(IWebHostEnvironment environment, AppDbContext db)
        {
            _environment = environment;
            _db = db;
        }


        [HttpPost]
        public async Task<IActionResult> CreatePost(IFormFileCollection fileCollection, string title, string content)
        {
            try
            {
                // Get user id from claims
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                // Validate user id
                if (string.IsNullOrEmpty(userId))
                    return StatusCode(StatusCodes.Status404NotFound, new { StatusCode = 404, MessageEn = "User ID not found", MessageAr = "لم يتم العثور على معرف المستخدم" });


                // Generate new post ID
                int postId = _db.Posts.OrderByDescending(p => p.PostId).Select(m => m.PostId).FirstOrDefault() + 1;

                // Create post entity
                var post = new Post
                {
                    Title = title,
                    Content = content,
                    UserId = userId,
                    Image = new List<string>(), // Initialize empty list for images
                    DateCreated = DateTimeHelper.FormatDate(DateTime.Now),
                    IsVisible = true,
                    TimeCreated = DateTimeHelper.FormatTime(DateTime.Now),
                    DateUpdated = "",
                    TimeUpdated = ""
                };

                // Add post to database
                _db.Posts.Add(post);
                await _db.SaveChangesAsync(); // Save changes asynchronously

                // Get file path
                string filePath = GetFilePath(userId, postId.ToString());

                // Upload images
                List<string> imagePaths = await UploadImages(fileCollection, filePath ,  userId);

                // Save image paths to the post entity
                post.Image.AddRange(imagePaths);

                // Update post in database to include image paths
                _db.Posts.Update(post);
                await _db.SaveChangesAsync();

                // Return successful response

                return StatusCode(StatusCodes.Status200OK, new { StatusCode = 404, MessageEn = "Post created successfully", MessageAr = "تم إنشاء المنشور بنجاح" });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { StatusCode = 500, MessageEn = $"An error occurred while saving the post. {ex}", MessageAr = $"حدث خطأ أثناء حفظ المنشور. {ex}" });
            }
        }

        private async Task<List<string>> UploadImages(IFormFileCollection fileCollection, string filePath, string userId)
        {
            List<string> imagePaths = new List<string>();

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            foreach (var file in fileCollection)
            {
                string fileName = Path.GetFileName(file.FileName).Replace(" ", ""); // Remove spaces from the file name
                string relativePath = $"/Post/{userId}/{Path.GetFileName(filePath)}/{fileName}";
                string imagePath = Path.Combine(filePath, fileName);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
                using (FileStream stream = System.IO.File.Create(imagePath))
                {
                    await file.CopyToAsync(stream);
                }
                imagePaths.Add(relativePath); // Add relative path to the list
            }

            return imagePaths;
        }


        private string GetFilePath(string userId, string postId)
        {
            return Path.Combine(_environment.WebRootPath, $"Post\\{userId}\\{postId}");
        }



    }
}
