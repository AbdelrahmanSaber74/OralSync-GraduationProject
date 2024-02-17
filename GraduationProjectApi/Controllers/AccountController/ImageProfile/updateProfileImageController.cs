using GraduationProjectApi.Models;
using IdentityManagerServerApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Security.Claims;

namespace GraduationProjectApi.Controllers.AccountController.ImageProfile
{
    [Route("api/[controller]")]
    [ApiController]
    public class updateProfileImageController : ControllerBase
    {
        private readonly IWebHostEnvironment environment;
        private readonly AppDbContext _db;
        public updateProfileImageController(IWebHostEnvironment environment, AppDbContext db)
        {
            this.environment = environment;
            _db = db;
        }
        


        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UploadImage(IFormFile formFile)
        {
            try
            {

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == userId);


                string Filepath = GetFilepath(userId);
                if (!System.IO.Directory.Exists(Filepath))
                {
                    System.IO.Directory.CreateDirectory(Filepath);
                }

                string imagepath = Filepath + "\\" + userId + ".png";
                if (System.IO.File.Exists(imagepath))
                {
                    System.IO.File.Delete(imagepath);
                }
                using (FileStream stream = System.IO.File.Create(imagepath))
                {
                    await formFile.CopyToAsync(stream);
                    user.ProfileImage = $"/Profile/{userId}/{userId}.png";
                    _db.SaveChanges();


                    return Ok("Done Profile Image Uploaad");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [NonAction]
        private string GetFilepath(string userId)
        {
            return this.environment.WebRootPath + $"\\Profile\\{userId}" ;
        }




    }
}
