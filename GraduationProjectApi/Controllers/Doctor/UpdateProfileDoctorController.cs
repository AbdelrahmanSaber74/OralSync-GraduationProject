using GraduationProjectApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SharedClassLibrary.DTOs;

namespace GraduationProjectApi.Controllers.Doctor
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UpdateProfileDoctorController : ControllerBase
    {
        private readonly IUpdateProfileDoctorService _updateProfileService;

        public UpdateProfileDoctorController(IUpdateProfileDoctorService updateProfileService)
        {
            _updateProfileService = updateProfileService;
        }

        [HttpPut]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> Put(DoctorDTO doctorDTO)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new { StatusCode = 400, MessageEn = "User ID not found in claims.", MessageAr = "معرف المستخدم غير موجود في البيانات." });
            }

            return await _updateProfileService.UpdateProfileAsync(doctorDTO, userId);
        }
    }
}
