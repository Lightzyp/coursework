using System.ComponentModel.DataAnnotations;

namespace Lab02Models.Models.Entities;

public class Employee

{
    public int Id { get; set; }

    [Required, StringLength(128)]
    public string FirstName { get; set; } = string.Empty;

    [Required, StringLength(128)]
    public string LastName { get; set; } = string.Empty;

    public int DepartmentId { get; set; }

    public Department? Department{ get; set; }

    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    public string FullName => $"{FirstName} {LastName}";

    public Locker? Locker { get; set; }

    public ICollection<Assignment> Assignment { get; set; }
    = new List<Assignment>();
    
}