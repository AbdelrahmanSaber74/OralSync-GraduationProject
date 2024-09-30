using GraduationProjectApi.Models;
using IdentityManagerServerApi.Data;
using Microsoft.EntityFrameworkCore;

namespace GraduationProjectApi.Services
{
    public class FilterDoctorsByGovernorateAndRateService : IFilterDoctorsByGovernorateAndRateService
    {
        private readonly AppDbContext _db;

        public FilterDoctorsByGovernorateAndRateService(AppDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<IEnumerable<object>> FilterDoctorsByGovernorateAndRateAsync(string governorate, double minRate, string hostUrl)
        {
            IQueryable<Doctor> doctorsQuery = _db.Doctors;

            if (!string.IsNullOrEmpty(governorate))
            {
                doctorsQuery = doctorsQuery.Where(d => d.Governorate == governorate);
            }

            var doctorsList = await doctorsQuery.ToListAsync();
            var doctorsWithRate = new List<object>();

            foreach (var doctor in doctorsList)
            {
                var averageRate = await CalculateAverageRate(doctor.UserId);
                if (averageRate >= minRate)
                {
                    doctorsWithRate.Add(new
                    {
                        Doctor = doctor,
                        profileImage = hostUrl + _db.Users.Where(m => m.Id == doctor.UserId).Select(m => m.ProfileImage).FirstOrDefault(),
                        Name = _db.Users.Where(m => m.Id == doctor.UserId).Select(m => m.Name).FirstOrDefault(),
                        AverageRate = averageRate
                    });
                }
            }

            return doctorsWithRate;
        }

        private async Task<double> CalculateAverageRate(string userId)
        {
            var ratedUserRatings = await _db.Ratings
                .Where(r => r.RatedUserId == userId)
                .Select(r => r.Value)
                .ToListAsync();

            return ratedUserRatings.Any() ? Math.Round(ratedUserRatings.Average(), 2) : 0;
        }
    }
}
