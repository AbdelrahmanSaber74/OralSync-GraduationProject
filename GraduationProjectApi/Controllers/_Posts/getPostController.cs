using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GraduationProjectApi.Models;
using System.Collections.Generic;
using IdentityManagerServerApi.Data;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Hosting;

namespace GraduationProjectApi.Controllers._Posts
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetPostController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly AppDbContext _db;

        public GetPostController(IWebHostEnvironment environment, AppDbContext db)
        {
            _environment = environment;
            _db = db;
        }

        [HttpGet("GetPostById")]
        public IActionResult get(string userId , int postId)
        {



            var posts = _db.Posts
             .Where(m => m.UserId == userId && m.PostId == postId)
             .ToList();

            string hosturl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";

            var images = posts.SelectMany(post => post.Image).ToList();

            List<string> combinedUrls = new List<string>();
            foreach (var image in images)
            {
                combinedUrls.Add(hosturl + image);
            }



            return Ok( new
            {
                posts ,
                PostImage = combinedUrls
            });


        }





        [HttpGet("GetMultiImage")]
        public async Task<IActionResult> GetMultiImage(string userId, string postId)
        {
            try
            {
                // Initialize a list to store image URLs
                List<string> imageUrls = new List<string>();

                // Construct the base URL of the API
                string baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

                // Get the file path for the images
                string filePath = GetFilePath(userId, postId);

                // Check if the directory exists
                if (Directory.Exists(filePath))
                {
                    // Get information about the directory
                    DirectoryInfo directoryInfo = new DirectoryInfo(filePath);

                    // Get all files in the directory
                    FileInfo[] fileInfos = directoryInfo.GetFiles();

                    // Iterate through each file
                    foreach (FileInfo fileInfo in fileInfos)
                    {
                        // Construct the full path of the image
                        string imagePath = Path.Combine(filePath, fileInfo.Name);

                        // Check if the file exists
                        if (System.IO.File.Exists(imagePath))
                        {
                            // Construct the image URL
                            string imageUrl = $"{baseUrl}/Post/{userId}/{postId}/{fileInfo.Name}";

                            // Add the image URL to the list
                            imageUrls.Add(imageUrl);
                        }
                    }
                }
                // Return the list of image URLs as a successful response
                return Ok(imageUrls);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }



        //[HttpGet("multiRemove")]
        //public async Task<IActionResult> multiremove(string userId, string postId)
        //{
        //    string Imageurl = string.Empty;
        //    string hosturl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
        //    try
        //    {
        //        string Filepath = GetFilepathPost(userId, postId);
        //        if (System.IO.Directory.Exists(Filepath))
        //        {
        //            DirectoryInfo directoryInfo = new DirectoryInfo(Filepath);
        //            FileInfo[] fileInfos = directoryInfo.GetFiles();
        //            foreach (FileInfo fileInfo in fileInfos)
        //            {
        //                fileInfo.Delete();
        //            }
        //            return Ok("pass");
        //        }
        //        else
        //        {
        //            return NotFound();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return NotFound();
        //    }


        //}





        private string GetFilePath(string userId, string postId)
        {
            return Path.Combine(_environment.WebRootPath, $"Post\\{userId}\\{postId}");
        }




    }
}
