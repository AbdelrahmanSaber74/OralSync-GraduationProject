using IdentityManagerServerApi.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace IdentityManagerServerApi.Models;

public class Student
{

    public int StudentId { get; set; }

    [Required]

    public string Name { get; set; }

    [Required]
    public bool? IsMale { get; set; }

    [Required]
    public string PhoneNumber { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public string UniversityName { get; set; }


    [Required]
    public double GPA { get; set; }

    [Required]
    public int AcademicYear { get; set; }


    [Required]
    public DateTime BirthDate { get; set; }

    [Required]
    public string UserId { get; set; }


}
