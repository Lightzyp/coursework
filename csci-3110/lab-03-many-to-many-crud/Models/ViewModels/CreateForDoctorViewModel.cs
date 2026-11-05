using DMLab3CRRUDM2M.Models.Entities;

namespace DMLab3CRRUDM2M.Models.ViewModels;

// The doctor we are booking for, plus every patient they could be booked with.
public class CreateForDoctorViewModel
{
    public Doctor Doctor { get; set; } = null!;
    public ICollection<Patient> Patients { get; set; } = new List<Patient>();
}
