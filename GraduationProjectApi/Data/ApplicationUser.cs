using GraduationProjectApi.Models;
using IdentityManagerServerApi.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityManagerServerApi.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }

        public DateTime TimeAddUser { get; set; }
        public ICollection<Post> Posts { get; set; } // Navigation property for posts authored by this user


    }
}
