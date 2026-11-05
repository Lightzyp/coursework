using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DMLab3CRRUDM2M.Models.ViewModels;

// Used by both the Setup (create) and Edit forms. Only Date and Notes are read from the form.
public class AppointmentFormViewModel
{
    // Nullable so an empty date field fails [Required] instead of silently becoming 01/01/0001.
    [Required(ErrorMessage = "Please enter the appointment date and time.")]
    [Display(Name = "Appointment Date")]
    [DataType(DataType.DateTime)]
    public DateTime? Date { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    // Display-only values filled in by the controller; [BindNever] stops users from posting them.
    [BindNever] public int AppointmentId { get; set; }
    [BindNever] public int DoctorId { get; set; }
    [BindNever] public int PatientId { get; set; }
    [BindNever] public string? DoctorName { get; set; }
    [BindNever] public string? PatientName { get; set; }

    // "Doctor" or "Patient": which side the user started from.
    [BindNever] public string? From { get; set; }
}
