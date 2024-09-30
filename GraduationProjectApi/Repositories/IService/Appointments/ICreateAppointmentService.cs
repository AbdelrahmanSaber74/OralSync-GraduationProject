using IdentityManagerServerApi.Models;
using SharedClassLibrary.DTOs;
using System.Threading.Tasks;

namespace GraduationProjectApi.Repositories.IService.Appointments
{
    public interface ICreateAppointmentService
    {
        Task<Appointment> CreateAppointmentAsync(AppointmentDto appointmentDto, string userId);
        Task<int> CountCompletedAppointmentsByUserAndRoleAsync(string userId, string role);
        Task<bool> CheckScheduledAppointmentExistsAsync(string userId);
    }
}
