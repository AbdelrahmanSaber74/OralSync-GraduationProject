using IdentityManagerServerApi.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IdentityManagerServerApi.Models;
public class Doctor
{
    public int DoctorId { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public bool IsMale { get; set; }

    [Required]
    public string PhoneNumber { get; set; }



    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string UniversityName { get; set; }

    [Required]
    public double GPA { get; set; }

    [Required]
    public string ClinicAddress { get; set; }

    [Required]
    public string ClinicNumber { get; set; }
  
    [Required]
    public List<string> InsuranceCompanies { get; set; }

    [Required]
    public List<string> Certificates { get; set; }

    [Required]
    public DateTime GraduationDate { get; set; }


    [Required]
    public DateTime BirthDate { get; set; }

    [Required]
    public string UserId { get; set; }


}
