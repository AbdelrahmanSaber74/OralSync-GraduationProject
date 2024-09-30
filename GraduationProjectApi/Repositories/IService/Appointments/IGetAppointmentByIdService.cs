using IdentityManagerServerApi.Models;

namespace GraduationProjectApi.Repositories.IService.Appointments
{
    public interface IGetAppointmentByIdService
    {
        Task<Appointment> GetAppointmentByIdAsync(int id);
    }
}
