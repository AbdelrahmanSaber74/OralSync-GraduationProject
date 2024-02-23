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
    public class ActiveAcountController : ControllerBase
    {
        private readonly IUserAccount _userAccount;
        private readonly AppDbContext _db;

        public ActiveAcountController(IUserAccount userAccount, AppDbContext db)
        {
            _userAccount = userAccount;
            _db = db;
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Activate(string email)
        {
            var user = _db.Users.FirstOrDefault(m => m.Email == email && !m.IsActive);
            if (user != null)
            {
                user.IsActive = true;
                _db.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { StatusCode = 200, MessageEn = "User activated successfully.", MessageAr = "تم تنشيط المستخدم بنجاح." });
            }
            return StatusCode(StatusCodes.Status404NotFound, new { StatusCode = 404, MessageEn = "User not found or already active.", MessageAr = "المستخدم غير موجود أو مفعل بالفعل." });
        }


    }
}
