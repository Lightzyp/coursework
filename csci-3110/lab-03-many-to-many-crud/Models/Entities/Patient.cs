using System.ComponentModel.DataAnnotations;

namespace DMLab3CRRUDM2M.Models.Entities;

public class Patient
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
