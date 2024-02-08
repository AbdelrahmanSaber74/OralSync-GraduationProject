using Microsoft.AspNetCore.Identity;

namespace IdentityManagerServerApi.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }

        public DateTime? TimeAddUser { get; set; }

    }
}
