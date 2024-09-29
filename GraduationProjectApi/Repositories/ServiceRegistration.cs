// Services/ServiceRegistration.cs
using GraduationProjectApi.Repositories;
using GraduationProjectApi.Services;
using Microsoft.Extensions.DependencyInjection;

namespace YourNamespace.Services
{
    public static class ServiceRegistration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {

            // Post Services
            services.AddScoped<IChangePostStatusService, ChangePostStatusService>();
            services.AddScoped<ICreatePostService, CreatePostService>();
            services.AddScoped<IDeletePostService, DeletePostService>();
            services.AddScoped<IGetAllHiddenPostsByUserService, GetAllHiddenPostsByUserService>();
            services.AddScoped<IGetAllPostByUserIdService, GetAllPostByUserIdService>();
            services.AddScoped<IGetAllPostService, GetAllPostService>();
            services.AddScoped<IGetAllPostsByUserService, GetAllPostsByUserService>();
            services.AddScoped<IGetPostByPostIdService, GetPostByPostIdService>();




           

            // Add other services here as needed
        }
    }
}
