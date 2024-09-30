using SharedClassLibrary.DTOs;

namespace GraduationProjectApi.Services
{
    public interface ISaveContactUsService
    {
        void SubmitContactUs(ContactUsDTO contactUs);
    }
}
