using IdentityManagerServerApi.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace IdentityManagerServerApi.Models;

public class Patient
{
    public int PatientId { get; set; }

    [Required]
    public string FirstName { get; set; }
    [Required]

    public string LastName { get; set; }

    [Required]
    public bool IsMale { get; set; }

    [Required]
    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public List<string>? Address { get; set; }

    public string? InsuranceCompany { get; set; }



    [Required]
    public string BirthDate { get; set; }


    [Required]
    public string Governorate { get; set; }
    public string UserId { get; set; }

    public ApplicationUser User { get; set; }


}
