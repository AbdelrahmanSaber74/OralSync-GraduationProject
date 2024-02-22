using IdentityManagerServerApi.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IdentityManagerServerApi.Models;
public class Doctor
{
    public int DoctorId { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    public bool IsMale { get; set; }

    [Required]
    public string PhoneNumber { get; set; }



    [Required]
    [EmailAddress]
    public string Email { get; set; }

    public string? UniversityName { get; set; }

    public double? GPA { get; set; }

    public List<string>? ClinicAddress { get; set; }

    public string? ClinicNumber { get; set; }
  
    public List<string>? InsuranceCompanies { get; set; }

    public List<string>? Certificates { get; set; }

    public string? GraduationDate { get; set; }



    [Required]  
    public string BirthDate { get; set; }


    public string UserId { get; set; }

    // Navigation property for the associated user
    public ApplicationUser User { get; set; }


}
