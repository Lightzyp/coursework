using DMLab3CRRUDM2M.Models.Entities;

namespace DMLab3CRRUDM2M.Models.ViewModels;

// The patient we are booking for, plus every doctor they could be booked with.
public class CreateForPatientViewModel
{
    public Patient Patient { get; set; } = null!;
    public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
}
