using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IdentityManagerServerApi.Models;
using GraduationProjectApi.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityManagerServerApi.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {

        //private readonly string connectionString;

        public AppDbContext()
        {
           // connectionString = "Data Source=SQL8005.site4now.net;Initial Catalog=db_aa4b69_graduationproject;User Id=db_aa4b69_graduationproject_admin;Password=Data Source=SQL8005.site4now.net;Initial Catalog=db_aa4b69_graduationproject;User Id=db_aa4b69_graduationproject_admin;Password=QxaN@qT23wLUL7t";
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }




        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Post> Posts { get; set; }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<ContactUs> ContactUs { get; set; }



        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer(connectionString);
        //    }
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {



            base.OnModelCreating(modelBuilder);

            // Cascading delete for Doctor entities
            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.User)                    // Each Doctor has one User
                .WithMany(u => u.Doctors)               // Each User can be associated with multiple Doctors
                .HasForeignKey(d => d.UserId)           // Foreign key linking Doctor to User
                .OnDelete(DeleteBehavior.Cascade);      // Cascade delete: if User is deleted, associated Doctor entities are also deleted

            // Cascading delete for Student entities
            modelBuilder.Entity<Student>()
                .HasOne(s => s.User)                    // Each Student has one User
                .WithMany(u => u.Students)              // Each User can be associated with multiple Students
                .HasForeignKey(s => s.UserId)           // Foreign key linking Student to User
                .OnDelete(DeleteBehavior.Cascade);      // Cascade delete: if User is deleted, associated Student entities are also deleted

            // Cascading delete for Patient entities
            modelBuilder.Entity<Patient>()
                .HasOne(p => p.User)                    // Each Patient has one User
                .WithMany(u => u.Patients)              // Each User can be associated with multiple Patients
                .HasForeignKey(p => p.UserId)           // Foreign key linking Patient to User
                .OnDelete(DeleteBehavior.Cascade);      // Cascade delete: if User is deleted, associated Patient entities are also deleted










            // One-to-many relationship between User and Post

                modelBuilder.Entity<Post>()
             .HasOne(p => p.User)                    // A post has one user
             .WithMany(u => u.Posts)                 // A user can have many posts
             .HasForeignKey(p => p.UserId)          // Define foreign key relationship
             .OnDelete(DeleteBehavior.Restrict);      // Cascade delete: if User is deleted, associated posts are also deleted

            modelBuilder.Entity<Like>()
                .HasOne(l => l.User)                    // A like has one user
                .WithMany(u => u.Likes)                 // A user can have many likes
                .HasForeignKey(l => l.UserId)          // Define foreign key relationship
                .OnDelete(DeleteBehavior.Restrict);      // Cascade delete: if User is deleted, associated likes are also deleted

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)                    // A comment has one user
                .WithMany(u => u.Comments)              // A user can have many comments
                .HasForeignKey(c => c.UserId)          // Define foreign key relationship
                .OnDelete(DeleteBehavior.Restrict);      // Cascade delete: if User is deleted, associated comments are also deleted



            // One-to-many relationship between Post and Comment

            modelBuilder.Entity<Comment>()
            .HasOne(c => c.Post)
            .WithMany(p => p.Comments)
            .HasForeignKey(c => c.PostId)
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete: if User is deleted, associated Patient entities are also deleted

            // One-to-many relationship between Post and Like

            modelBuilder.Entity<Like>()
                .HasOne(l => l.Post)
                .WithMany(p => p.Likes)
                .HasForeignKey(l => l.PostId)
                .OnDelete(DeleteBehavior.Cascade);  // Cascade delete: if User is deleted, associated Patient entities are also deleted



        }
    }
}
    