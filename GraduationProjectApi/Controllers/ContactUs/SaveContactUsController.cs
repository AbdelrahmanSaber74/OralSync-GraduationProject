using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedClassLibrary.DTOs;
using GraduationProjectApi.Services;

namespace GraduationProjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaveContactUsController : ControllerBase
    {
        private readonly ISaveContactUsService _contactUsService;

        public SaveContactUsController(ISaveContactUsService contactUsService)
        {
            _contactUsService = contactUsService ?? throw new ArgumentNullException(nameof(contactUsService));
        }

        [HttpPut]
        [Authorize]
        public IActionResult Put([FromBody] ContactUsDTO contactUs)
        {
            if (contactUs == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { StatusCode = 400, MessageEn = "Invalid contact form data", MessageAr = "بيانات الرسالة غير صالحة" });
            }

            try
            {
                _contactUsService.SubmitContactUs(contactUs);
                return StatusCode(StatusCodes.Status200OK, new { StatusCode = 200, MessageEn = "Contact form submitted successfully", MessageAr = "تم حفظ الرسالة بنجاح" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { StatusCode = 500, MessageEn = $"Internal Server Error: {ex.Message}", MessageAr = "حدث خطأ داخلي في الخادم" });
            }
        }
    }
}
