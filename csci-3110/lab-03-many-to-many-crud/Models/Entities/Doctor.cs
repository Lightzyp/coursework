using System.ComponentModel.DataAnnotations;

namespace DMLab3CRRUDM2M.Models.Entities;

public class Doctor
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(100)]
    public string? Specialty { get; set; }

    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
