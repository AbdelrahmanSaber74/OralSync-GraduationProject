using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraduationProjectApi.Repositories.IService.Appointments
{
    public interface IGetCompletedPatientAppointmentByUserIdService
    {
        Task<IEnumerable<object>> GetCompletedAppointmentsAsync(string userId, string hostUrl);
    }
}
