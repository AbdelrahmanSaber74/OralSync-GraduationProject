using GraduationProjectApi.Models;
using IdentityManagerServerApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedClassLibrary.DTOs;
using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GraduationProjectApi.Controllers.AccountController.ImageProfile
{
    [Route("api/[controller]")]
    [ApiController]
    public class RemoveProfileImageController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly AppDbContext _db;

        public RemoveProfileImageController(IWebHostEnvironment environment, AppDbContext db)
        {
            _environment = environment;
            _db = db;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Remove(bool isMale)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == userId);

                if (user == null)
                    return StatusCode(StatusCodes.Status404NotFound, new { StatusCode = 404, MessageEn = "User not found.", MessageAr = "المستخدم غير موجود." });

                string defaultImage = isMale ? "male.png" : "female.png";

                string filePath = GetFilePath(userId);
                string imagePath = Path.Combine(filePath, $"{userId}.png");

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                    user.ProfileImage = $"/Profile/default/{defaultImage}";
                    await _db.SaveChangesAsync();
                    return StatusCode(StatusCodes.Status200OK, new { StatusCode = 200, MessageEn = "Profile image uploaded successfully.", MessageAr = "تم تحميل صورة الملف الشخصي بنجاح." });
                }
                else
                {
                    return NotFound("Profile image not found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { StatusCode = 400, MessageEn = $"Failed to upload profile image: {ex.Message}.", MessageAr = "فشل تحميل صورة الملف الشخصي." });
            }
        }

        [NonAction]
        private string GetFilePath(string userId)
        {
            return Path.Combine(_environment.WebRootPath, $"Profile\\{userId}");
        }




    }
}
