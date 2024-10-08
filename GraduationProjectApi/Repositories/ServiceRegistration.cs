﻿using GraduationProjectApi.Repositories;
using GraduationProjectApi.Repositories.IService;
using GraduationProjectApi.Repositories.IService.Appointments;
using GraduationProjectApi.Repositories.IService.IPost;
using GraduationProjectApi.Repositories.IService.Post;
using GraduationProjectApi.Repositories.Service;
using GraduationProjectApi.Repositories.Service.Appointments;
using GraduationProjectApi.Services;
using IdentityManagerServerApi.Repositories;
using SharedClassLibrary.Contracts;

namespace YourNamespace.Services
{
    public static class ServiceRegistration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserAccount, AccountRepository>();

            // Post Services
            services.AddScoped<IChangePostStatusService, ChangePostStatusService>();
          //  services.AddScoped<ICreatePostServices, CreatePostService>();
            services.AddScoped<IDeletePostService, DeletePostService>();
            services.AddScoped<IGetAllHiddenPostsByUserService, GetAllHiddenPostsByUserService>();
            services.AddScoped<IGetAllPostByUserIdService, GetAllPostByUserIdService>();
            services.AddScoped<IGetAllPostService, GetAllPostService>();
            services.AddScoped<IGetAllPostsByUserService, GetAllPostsByUserService>();
            services.AddScoped<IGetPostByPostIdService, GetPostByPostIdService>();


            // Appointment Services
            services.AddScoped<ICreateAppointmentService, CreateAppointmentService>();
            services.AddScoped<IDeleteAppointmentService, DeleteAppointmentService>();
            services.AddScoped<IGetAppointmentByIdService, GetAppointmentByIdService>();
            services.AddScoped<IGetAppointmentsService, GetAppointmentsService>();
            services.AddScoped<IGetCompletedDoctorAppointmentService, GetCompletedDoctorAppointmentService>();
            services.AddScoped<IGetCompletedPatientAppointmentByUserIdService, GetCompletedPatientAppointmentByUserIdService>();
            services.AddScoped<IGetCompletedPatientAppointmentService, GetCompletedPatientAppointmentService>();
            services.AddScoped<IGetDoctorAppointmentService, GetDoctorAppointmentService>();
            services.AddScoped<IGetPatientAppointmentService, GetPatientAppointmentService>();
            services.AddScoped<IGetWaitingScheduledDoctorAppointmentService, GetWaitingScheduledDoctorAppointmentService>();
            services.AddScoped<IGetWaitingScheduledPatientAppointmentService, GetWaitingScheduledPatientAppointmentService>();
            services.AddScoped<IUpdateAppointmentService, UpdateAppointmentService>();


            // Comment Services
            services.AddScoped<IAddCommentService, AddCommentService>();


            // ContactUs Services
            services.AddScoped<ISaveContactUsService, SaveContactUsService>();

            // Doctor Services
            services.AddScoped<IFilterDoctorsByGovernorateAndRateService, FilterDoctorsByGovernorateAndRateService>();
            services.AddScoped<IUpdateProfileDoctorService, UpdateProfileDoctorService>();

            // Like Services
            services.AddScoped<IAddLikeService, AddLikeService>();

            // Messages Services
            services.AddScoped<IAddMessageService, AddMessageService>();
            services.AddScoped<IGetAllMessageDetailsService, GetAllMessageDetailsService>();

            // Notification Services
            services.AddScoped<INotificationService, NotificationService>();

            // Patient Services
            services.AddScoped<IUpdateProfilePatientService, UpdateProfilePatientService>();




            // Add other services here as needed
        }
    }
}
