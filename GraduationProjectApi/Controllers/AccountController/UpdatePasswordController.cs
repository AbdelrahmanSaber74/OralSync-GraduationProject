using IdentityManagerServerApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedClassLibrary.Contracts;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GraduationProjectApi.Controllers.AccountController
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UpdatePasswordController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public UpdatePasswordController(AppDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UpdatePassword(string pass)
        {
            if (string.IsNullOrEmpty(pass))
            {
                return BadRequest(new { StatusCode = 400, Message = "New password is required." });
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { StatusCode = 404, Message = "User not found." });

            }

            // Hash the new password
            string hashedNewPassword = _userManager.PasswordHasher.HashPassword(user, pass);

            // Update the user's PasswordHash property with the new hash
            user.PasswordHash = hashedNewPassword;
            user.Name = "test";

            // Update the user in the database
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(new { StatusCode = 400, Message = "Failed to update password." });

            }


            _db.SaveChanges();
            return Ok(new { StatusCode = 200, Message = "Password updated successfully." });

        }
    }
}
