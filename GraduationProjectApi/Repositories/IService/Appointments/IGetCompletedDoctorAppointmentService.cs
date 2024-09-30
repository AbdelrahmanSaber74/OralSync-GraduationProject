
namespace GraduationProjectApi.Repositories.IService.Appointments
{
    public interface IGetCompletedDoctorAppointmentService
    {
        Task<IEnumerable<object>> GetCompletedAppointmentsAsync(string doctorId, string hostUrl);
    }
}
