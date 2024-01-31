using IdentityManagerServerApi.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace IdentityManagerServerApi.Models;

public class Patient
{
    public int PatientId { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public bool IsMale { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public string PhoneNumber { get; set; }

    [Required]
    public string Address { get; set; }

    public string InsuranceCompany { get; set; }

    [Required]
    public DateTime BirthDate { get; set; }
    public string? UserId { get; set; }


}
