using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GraduationProjectApi.Repositories.IService.Appointments;
using IdentityManagerServerApi.Models;

namespace GraduationProjectApi.Controllers.Appointments
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetAppointmentsController : ControllerBase
    {
        private readonly IGetAppointmentsService _getAppointmentsService;

        public GetAppointmentsController(IGetAppointmentsService getAppointmentsService)
        {
            _getAppointmentsService = getAppointmentsService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointments()
        {
            var appointments = await _getAppointmentsService.GetAllAppointmentsAsync();
            return Ok(appointments);
        }
    }
}
