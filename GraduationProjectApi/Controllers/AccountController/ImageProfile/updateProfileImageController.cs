using GraduationProjectApi.Models;
using IdentityManagerServerApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GraduationProjectApi.Controllers.AccountController.ImageProfile
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateProfileImageController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly AppDbContext _db;

        public UpdateProfileImageController(IWebHostEnvironment environment, AppDbContext db)
        {
            _environment = environment;
            _db = db;
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UploadImage(IFormFile formFile)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == userId);

                if (user == null)
                    return StatusCode(StatusCodes.Status404NotFound, new { StatusCode = 404, MessageEn = "User not found.", MessageAr = "المستخدم غير موجود." });

                // Use the original file name for the uploaded image
                string filePath = GetFilePath(userId);
                string imageFileName = Path.GetFileName(formFile.FileName);



                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);

                string imagePath = Path.Combine(filePath, $"{userId}.png");

                if (System.IO.File.Exists(imagePath))
                    System.IO.File.Delete(imagePath);

                using (FileStream stream = System.IO.File.Create(imagePath))
                {
                    await formFile.CopyToAsync(stream);
                    user.ProfileImage = $"/Profile/{userId}/{imageFileName}.png";
                    await _db.SaveChangesAsync();


                    return StatusCode(StatusCodes.Status200OK, new { StatusCode = 200, MessageEn = "Profile image uploaded successfully.", MessageAr = "تم تحميل صورة الملف الشخصي بنجاح." });

                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { StatusCode = 400, MessageEn = $"Failed to upload profile image: {ex.Message}.", MessageAr = "فشل تحميل صورة الملف الشخصي." });

            }
        }

        private string GetFilePath(string userId)
        {
            return Path.Combine(_environment.WebRootPath, $"Profile\\{userId}");
        }



    }
}
