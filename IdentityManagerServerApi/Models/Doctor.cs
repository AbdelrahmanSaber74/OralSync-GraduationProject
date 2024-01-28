using IdentityManagerServerApi.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IdentityManagerServerApi.Models;
public class Doctor
{
    [Key]
    public int DoctorId { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }


    public string Specialization { get; set; }

}
