using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IdentityManagerServerApi.Models;
using GraduationProjectApi.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityManagerServerApi.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {

        private readonly string connectionString;

        public AppDbContext()
        {
            connectionString = "Data Source=SQL8005.site4now.net;Initial Catalog=db_aa4b69_graduationproject;User Id=db_aa4b69_graduationproject_admin;Password=Data Source=SQL8005.site4now.net;Initial Catalog=db_aa4b69_graduationproject;User Id=db_aa4b69_graduationproject_admin;Password=QxaN@qT23wLUL7t";
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }




        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Post> Posts { get; set; }




        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);






            modelBuilder.Entity<Post>()
                .HasOne(p => p.User)        // A post has one user
                .WithMany(u => u.Posts)     // A user can have many posts
                .HasForeignKey(p => p.UserId) // Define foreign key relationship
                .IsRequired();              // Make UserId required






        }
    }
}
    