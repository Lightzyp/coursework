using System.ComponentModel.DataAnnotations;

namespace Lab02Models.Models.Entities;

public class Locker

{
    public int Id {get; set;}

    [Required, StringLength(20)]
    public string Number {get; set;} = string.Empty;

    public int EmployeeId {get; set;}

    public Employee? Employee {get; set;}
}