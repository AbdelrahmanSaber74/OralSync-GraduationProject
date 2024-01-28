using IdentityManagerServerApi.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace IdentityManagerServerApi.Models;

public class Student
{
    [Key]
    public int StudentId { get; set; }

    public string Name { get; set; }

    public string UnvirestyName { get; set; }

    public string Description { get; set; }

}
