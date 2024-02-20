using GraduationProjectApi.Models;
using IdentityManagerServerApi.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityManagerServerApi.Data
{
    public class ApplicationUser : IdentityUser
    {


        public string Name { get; set; }


        public string? ProfileImage { get; set; }

        public DateTime TimeAddUser { get; set; }


        public ICollection<Post> Posts { get; set; } // Navigation property for posts authored by this user


        // Navigation property for doctors associated with this user
        public ICollection<Doctor> Doctors { get; set; }
        public ICollection<Student> Students { get; set; }
        public ICollection<Patient> Patients { get; set; }
        public ICollection<Like> Likes { get; set; }


    }
}
