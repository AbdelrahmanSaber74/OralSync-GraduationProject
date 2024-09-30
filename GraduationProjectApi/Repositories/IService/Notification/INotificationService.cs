using IdentityManagerServerApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraduationProjectApi.Repositories
{
    public interface INotificationService
    {
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(string userId);
    }
}
