using GraduationProjectApi.Models;
using IdentityManagerServerApi.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityManagerServerApi.Data
{
    public class ApplicationUser : IdentityUser
    {


        public string Name { get; set; }


        public string? ProfileImage { get; set; }
        public bool IsActive { get; set; }
        public int Rate { get; set; } = 3 ;
        public DateTime TimeAddUser { get; set; }




        // Navigation property for doctors associated with this user
        public ICollection<Doctor> Doctors { get; set; }
        public ICollection<Student> Students { get; set; }
        public ICollection<Patient> Patients { get; set; }


        public  ICollection<Post> Posts { get; set; }
        public  ICollection<Like> Likes { get; set; }
        public  ICollection<Comment> Comments { get; set; }


    }
}
