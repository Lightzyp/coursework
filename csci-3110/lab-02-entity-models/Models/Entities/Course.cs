using System.ComponentModel.DataAnnotations;
namespace Lab02Models.Models.Entities;
public class Course
{
    [Required, StringLength(4)]
    public string Code { get; set; } = string.Empty;
    [Required, StringLength(4)]
    public string Number { get; set; } = string.Empty;
    [Range(1, 6)]
    public int CreditHours { get; set; }
}
