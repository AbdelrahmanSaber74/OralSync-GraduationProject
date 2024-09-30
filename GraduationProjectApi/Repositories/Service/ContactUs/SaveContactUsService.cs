using GraduationProjectApi.Models;
using IdentityManagerServerApi.Data;
using SharedClassLibrary.DTOs;
using SharedClassLibrary.Helper;
using System;

namespace GraduationProjectApi.Services
{
    public class SaveContactUsService : ISaveContactUsService
    {
        private readonly AppDbContext _db;

        public SaveContactUsService(AppDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public void SubmitContactUs(ContactUsDTO contactUs)
        {
            var newRequest = new ContactUs
            {
                FullName = contactUs.FullName,
                Email = contactUs.Email,
                PhoneNumber = contactUs.PhoneNumber,
                Message = contactUs.Message,
                DateCreated = DateTimeHelper.FormatDate(DateTime.Now),
                TimeCreated = DateTimeHelper.FormatTime(DateTime.Now),
            };

            _db.Add(newRequest);
            _db.SaveChanges();
        }
    }
}
