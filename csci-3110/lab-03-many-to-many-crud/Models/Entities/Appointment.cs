using System.ComponentModel.DataAnnotations;

namespace DMLab3CRRUDM2M.Models.Entities;

public class Appointment
{
    public int Id { get; set; }

    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; } = null!;

    public int PatientId { get; set; }
    public Patient Patient { get; set; } = null!;

    [Display(Name = "Appointment Date")]
    [DataType(DataType.DateTime)]
    public DateTime Date { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }
}
