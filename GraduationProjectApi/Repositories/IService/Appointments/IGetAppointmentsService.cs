using GraduationProjectApi.Models;
using IdentityManagerServerApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraduationProjectApi.Repositories.IService.Appointments
{
    public interface IGetAppointmentsService
    {
        Task<IEnumerable<Appointment>> GetAllAppointmentsAsync();
    }
}
