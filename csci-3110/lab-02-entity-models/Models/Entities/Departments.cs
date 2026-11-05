using System.ComponentModel.DataAnnotations;

namespace Lab02Models.Models.Entities;

public class Department
{
    public int Id {get; set; }

    [Required, StringLength(100)]
    public string Name {get; set;} = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal Budget {get; set;}

    public ICollection<Employee> Employee {get; set;}
    = new List<Employee>();
}

