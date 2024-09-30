using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraduationProjectApi.Repositories.IService.Appointments
{
    public interface IGetDoctorAppointmentService
    {
        Task<IEnumerable<object>> GetDoctorAppointmentsAsync(string userId, string hostUrl);
    }
}
