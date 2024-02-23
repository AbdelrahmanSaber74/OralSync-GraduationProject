using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GraduationProjectApi.Models;
using System;
using System.Linq;
using IdentityManagerServerApi.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace GraduationProjectApi.Controllers._Posts
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, Doctor, Student")]
    public class ChangePostStatusController : ControllerBase
    {
        private readonly AppDbContext _db;

        public ChangePostStatusController(AppDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }



        [HttpPut]
        public IActionResult ChangeStatus(int postId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return StatusCode(StatusCodes.Status404NotFound, new { StatusCode = 404, MessageEn = "User ID not found", MessageAr = "لم يتم العثور على معرف المستخدم" });
            }

            // Retrieve the post from the database
            var post = _db.Posts.FirstOrDefault(p => p.PostId == postId && p.UserId == userId);

            if (post == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { StatusCode = 404, MessageEn = "Post not found", MessageAr = "لم يتم العثور على المنشور" });
            }

            // Toggle the post visibility status
            post.IsVisible = !post.IsVisible;

            _db.SaveChanges();

            string statusMessageEn = post.IsVisible ? "Post is now visible" : "Post is now hidden";
            string statusMessageAr = post.IsVisible ? "المنشور مرئي الآن" : "المنشور مخفي الآن";

            return StatusCode(StatusCodes.Status200OK, new { StatusCode = 200, MessageEn = statusMessageEn, MessageAr = statusMessageAr });
        }

    }
}
