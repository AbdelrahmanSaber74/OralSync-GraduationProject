using GraduationProjectApi.Models;
using IdentityManagerServerApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
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
            if (formFile == null || formFile.Length == 0)
            {
                return BadRequest(new { StatusCode = 400, MessageEn = "No file provided.", MessageAr = "لم يتم توفير أي ملف." });
            }

            var validImageTypes = new[] { "image/jpeg", "image/png", "image/gif" };
            if (!validImageTypes.Contains(formFile.ContentType))
            {
                return BadRequest(new { StatusCode = 400, MessageEn = "Invalid file type.", MessageAr = "نوع الملف غير صالح." });
            }

            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == userId);

                if (user == null)
                {
                    return NotFound(new { StatusCode = 404, MessageEn = "User not found.", MessageAr = "المستخدم غير موجود." });
                }

                string filePath = GetFilePath(userId);
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                // Sanitize the file name
                string originalFileName = Path.GetFileName(formFile.FileName);
                string sanitizedFileName = Regex.Replace(originalFileName.Trim(), @"[^a-zA-Z0-9_.-]", "_");

                string imagePath = Path.Combine(filePath, sanitizedFileName);

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }

                user.ProfileImage = $"/Profile/{userId}/{sanitizedFileName}";
                await _db.SaveChangesAsync();

                return Ok(new { StatusCode = 200, MessageEn = "Profile image uploaded successfully.", MessageAr = "تم تحميل صورة الملف الشخصي بنجاح." });
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here using a logging framework
                return BadRequest(new { StatusCode = 400, MessageEn = $"Failed to upload profile image: {ex.Message}", MessageAr = "فشل تحميل صورة الملف الشخصي." });
            }
        }

        private string GetFilePath(string userId)
        {
            // Ensure the userId is safe to use in a path
            var safeUserId = Path.GetInvalidFileNameChars().Aggregate(userId, (current, c) => current.Replace(c.ToString(), string.Empty));
            return Path.Combine(_environment.WebRootPath, "Profile", safeUserId);
        }
    }
}
