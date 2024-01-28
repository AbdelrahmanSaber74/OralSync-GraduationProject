using IdentityManagerServerApi.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace IdentityManagerServerApi.Models;

public class Patient
{
    [Key]
    public int PatientId { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }
    public string HealthCondition { get; set; }


}
