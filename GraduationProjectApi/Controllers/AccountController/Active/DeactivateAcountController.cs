using IdentityManagerServerApi.Data;
using IdentityManagerServerApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedClassLibrary.Contracts;
using SharedClassLibrary.DTOs;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProjectApi.Controllers.AccountController.Active
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeactivateAcountController : ControllerBase
    {
        private readonly IUserAccount _userAccount;
        private readonly AppDbContext _db;

        public DeactivateAcountController(IUserAccount userAccount, AppDbContext db)
        {
            _userAccount = userAccount;
            _db = db;
        }

        
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Deactivate(string email)
        {
            var user = _db.Users.FirstOrDefault(m => m.Email == email && m.IsActive);
            if (user != null)
            {
                user.IsActive = false;
                _db.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { StatusCode = 200, MessageEn = "User deactivated successfully.", MessageAr = "تم إلغاء تنشيط المستخدم بنجاح." });
            }
            return StatusCode(StatusCodes.Status404NotFound, new { StatusCode = 404, MessageEn = "User not found or already inactive.", MessageAr = "المستخدم غير موجود أو غير نشط بالفعل." });
        }


    }
}
