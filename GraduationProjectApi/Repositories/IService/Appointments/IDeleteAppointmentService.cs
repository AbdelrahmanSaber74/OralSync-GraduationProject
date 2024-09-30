namespace GraduationProjectApi.Repositories.IService.Appointments
{
    public interface IDeleteAppointmentService
    {
        Task<bool> DeleteAppointmentAsync(int id);
    }
}
