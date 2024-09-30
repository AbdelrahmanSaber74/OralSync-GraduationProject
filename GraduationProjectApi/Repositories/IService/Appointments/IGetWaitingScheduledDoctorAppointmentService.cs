using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraduationProjectApi.Repositories.IService.Appointments
{
    public interface IGetWaitingScheduledDoctorAppointmentService
    {
        Task<IEnumerable<object>> GetWaitingScheduledAppointmentsAsync(string userId, string hostUrl);
    }
}
