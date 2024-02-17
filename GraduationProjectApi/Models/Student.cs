using IdentityManagerServerApi.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace IdentityManagerServerApi.Models;

public class Student
{

    public int StudentId { get; set; }


    [Required]
    public string FirstName { get; set; }
    [Required]

    public string LastName { get; set; }


    [Required]
    public bool? IsMale { get; set; }

    [Required]
    public string PhoneNumber { get; set; }

    [Required]
    public string Email { get; set; }

    public string? UniversityName { get; set; }

    public List<string>? UniversitAddress { get; set; }

    public double? GPA { get; set; }

    public int? AcademicYear { get; set; }





    [Required]
    public string BirthDate { get; set; }

    [Required]
    public string UserId { get; set; }


}
