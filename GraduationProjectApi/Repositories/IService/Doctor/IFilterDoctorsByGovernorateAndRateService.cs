namespace GraduationProjectApi.Services
{
    public interface IFilterDoctorsByGovernorateAndRateService
    {
        Task<IEnumerable<object>> FilterDoctorsByGovernorateAndRateAsync(string governorate, double minRate, string hostUrl);
    }
}
