using GraduationProjectApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProjectApi.Controllers.Doctor
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilterDoctorsByGovernorateAndRateController : ControllerBase
    {
        private readonly IFilterDoctorsByGovernorateAndRateService _filterDoctorsService;

        public FilterDoctorsByGovernorateAndRateController(IFilterDoctorsByGovernorateAndRateService filterDoctorsService)
        {
            _filterDoctorsService = filterDoctorsService ?? throw new ArgumentNullException(nameof(filterDoctorsService));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Get(string governorate = null, double minRate = 0)
        {
            try
            {
                string hostUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
                var doctorsWithRate = await _filterDoctorsService.FilterDoctorsByGovernorateAndRateAsync(governorate, minRate, hostUrl);
                return Ok(doctorsWithRate);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = 500,
                    MessageEn = $"An error occurred while filtering doctors by governorate and rate. {ex.Message}",
                    MessageAr = "حدث خطأ أثناء تصفية الأطباء حسب المحافظة والتقييم."
                });
            }
        }
    }
}
