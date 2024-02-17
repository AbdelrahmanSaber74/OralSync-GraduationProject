using GraduationProjectApi.Models;
using IdentityManagerServerApi.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace LearnAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageForPostController : ControllerBase
    {
        private readonly IWebHostEnvironment environment;
        private readonly AppDbContext _db;
        public ImageForPostController(IWebHostEnvironment environment, AppDbContext db)
        {
            this.environment = environment;
            _db = db;
        }

     

        [HttpPut("MultiUploadImage")]
        public async Task<IActionResult> MultiUploadImagePost(IFormFileCollection filecollection, string userId, string postId)
        {
            int passcount = 0;
            try
            {

                string Filepath = GetFilepathPost(userId, postId); // Modified to include userId and postId in the file path


                if (!System.IO.Directory.Exists(Filepath))
                {
                    System.IO.Directory.CreateDirectory(Filepath);
                }
                foreach (var file in filecollection)
                {


                    string imagepath = Filepath + "\\" + file.FileName.Replace(" ", "");
                    if (System.IO.File.Exists(imagepath))
                    {
                        System.IO.File.Delete(imagepath);
                    }
                    using (FileStream stream = System.IO.File.Create(imagepath))
                    {
                        await file.CopyToAsync(stream);
                        passcount++;

                    }
                }


            }
            catch (Exception ex)
            {
                return BadRequest("bad");
            }
            return Ok($"Successfully uploaded {passcount} files.");
        }





        [HttpGet("GetMultiImage")]
        public async Task<IActionResult> GetMultiImage(string userId, string postId)
        {
            List<string> Imageurl = new List<string>();
            string hosturl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            try
            {
                string Filepath = GetFilepathPost(userId, postId);

                if (System.IO.Directory.Exists(Filepath))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(Filepath);
                    FileInfo[] fileInfos = directoryInfo.GetFiles();
                    foreach (FileInfo fileInfo in fileInfos)
                    {
                        string filename = fileInfo.Name;
                        string imagepath = Filepath + "\\" + filename;
                        if (System.IO.File.Exists(imagepath))
                        {
                            string _Imageurl = hosturl + $"/Post/{userId}/{postId}" + "/" + filename;
                            Imageurl.Add(_Imageurl);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
            }
            return Ok(Imageurl);

        }



        [HttpGet("multiRemove")]
        public async Task<IActionResult> multiremove(string userId, string postId)
        {
            string Imageurl = string.Empty;
            string hosturl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            try
            {
                string Filepath = GetFilepathPost(userId , postId);
                if (System.IO.Directory.Exists(Filepath))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(Filepath);
                    FileInfo[] fileInfos = directoryInfo.GetFiles();
                    foreach (FileInfo fileInfo in fileInfos)
                    {
                        fileInfo.Delete();
                    }
                    return Ok("pass");
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return NotFound();
            }


        }







        [NonAction]
        private string GetFilepathPost(string userId, string postId)
        {
            return this.environment.WebRootPath + $"\\Post\\{userId}\\{postId}\\";
        }




    }
}
