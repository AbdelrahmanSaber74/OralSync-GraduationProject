using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraduationProjectApi.Repositories.IService.Appointments
{
    public interface IGetPatientAppointmentService
    {
        Task<IEnumerable<object>> GetPatientAppointmentsAsync(string userId, string hostUrl);
    }
}
