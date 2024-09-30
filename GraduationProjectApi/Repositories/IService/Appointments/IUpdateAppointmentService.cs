using System.Threading.Tasks;
using SharedClassLibrary.DTOs;

namespace GraduationProjectApi.Repositories.IService.Appointments
{
    public interface IUpdateAppointmentService
    {
        Task<bool> UpdateAppointmentAsync(int id, AppointmentUpdateDto appointmentUpdateDto);
    }
}
