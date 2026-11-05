using DMLab3CRRUDM2M.Models.Entities;

namespace DMLab3CRRUDM2M.Services;

public interface IAppointmentRepository
{
    Appointment Create(Appointment appointment);
    Appointment? Read(int id);
    ICollection<Appointment> ReadAll();
    bool Update(int id, Appointment appointment);
    bool Delete(int id);
}
