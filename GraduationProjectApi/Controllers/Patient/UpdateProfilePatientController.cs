using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedClassLibrary.DTOs;
using System.Security.Claims;

namespace GraduationProjectApi.Controllers.Students
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Patient")]
    public class UpdateProfilePatientController : ControllerBase
    {
        private readonly IUpdateProfilePatientService _updateProfilePatientService;

        public UpdateProfilePatientController(IUpdateProfilePatientService updateProfilePatientService)
        {
            _updateProfilePatientService = updateProfilePatientService;
        }

        [HttpPut]
        public async Task<IActionResult> Put(PatientDTO patientDTO)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new { StatusCode = 400, MessageEn = "User ID not found in claims.", MessageAr = "معرف المستخدم غير موجود في البيانات." });
            }

            try
            {
                await _updateProfilePatientService.UpdatePatientProfileAsync(patientDTO, userId);
                return Ok(new { StatusCode = 200, MessageEn = "Patient profile updated successfully.", MessageAr = "تم تحديث ملف المريض بنجاح." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { StatusCode = 404, MessageEn = ex.Message, MessageAr = "لم يتم العثور على المريض أو المستخدم." });
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, new { StatusCode = 500, MessageEn = "An error occurred while updating the Patient profile.", MessageAr = "حدث خطأ أثناء تحديث ملف المريض." });
            }
        }
    }
}
