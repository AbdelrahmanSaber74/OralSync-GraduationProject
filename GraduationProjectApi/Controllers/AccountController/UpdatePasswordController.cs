using IdentityManagerServerApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedClassLibrary.Contracts;
using SharedClassLibrary.DTOs;
using System.ComponentModel.DataAnnotations;
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

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    MessageEn = "User not found.",
                    MessageAr = "المستخدم غير موجود."
                });
            }

            // Validate the complexity of the new password
            var passwordValidator = _userManager.PasswordValidators.FirstOrDefault();
            if (passwordValidator != null)
            {
                var validationResult = await passwordValidator.ValidateAsync(_userManager, user, model.NewPassword);
                if (!validationResult.Succeeded)
                {
                    var errors = validationResult.Errors.Select(e => e.Description);
                    return BadRequest(new
                    {
                        StatusCode = 400,
                        MessageEn = "Password complexity requirements not met.",
                        MessageAr = "لم يتم تلبية متطلبات تعقيد كلمة المرور.",
                        Errors = errors
                    });
                }
            }

            // Hash the new password
            string hashedNewPassword = _userManager.PasswordHasher.HashPassword(user, model.NewPassword);

            // Update the user's PasswordHash property with the new hash
            user.PasswordHash = hashedNewPassword;

            // Update the user in the database
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    MessageEn = "Failed to update password.",
                    MessageAr = "فشل تحديث كلمة المرور."
                });
            }

            await _db.SaveChangesAsync();

            return Ok(new
            {
                StatusCode = 200,
                MessageEn = "Password updated successfully.",
                MessageAr = "تم تحديث كلمة المرور بنجاح."
            });
        }
    }

 
}
