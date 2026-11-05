using System.ComponentModel.DataAnnotations;

namespace Lab02Models.Models.Entities;

public class Assignment
{
    // An assignment is identified by its employee and project keys.


    public int EmployeeId { get; set; }
    public Employee? Employee { get; set; }

    public int ProjectId { get; set; }
    public Project? Project { get; set; }

    [Required, StringLength(100)]
    public string Role { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal Hours { get; set; }
}