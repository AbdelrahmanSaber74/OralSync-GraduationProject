using IdentityManagerServerApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GraduationProjectApi.Controllers.AccountController
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeleteAcoountController : ControllerBase
    {
        private readonly AppDbContext _db;

        public DeleteAcoountController(AppDbContext db)
        {
            _db = db;
        }


        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Delete()
        {


            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            var user = _db.Users.Where(m => m.Id == userId).FirstOrDefault();
                
            if (user == null)
            {
                return NotFound(new { StatusCode = 404, MessageEn = "User not found.", MessageAr = "المستخدم غير موجود." });
            }

            _db.Users.Remove(user);
            await _db.SaveChangesAsync();

            return StatusCode(StatusCodes.Status200OK, new { StatusCode = 200, MessageEn = "User deleted successfully.", MessageAr = "تم حذف المستخدم بنجاح." });

        }



    }
}
