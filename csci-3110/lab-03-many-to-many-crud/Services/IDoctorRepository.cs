using DMLab3CRRUDM2M.Models.Entities;

namespace DMLab3CRRUDM2M.Services;

public interface IDoctorRepository
{
    Doctor Create(Doctor doctor);
    Doctor? Read(int id);
    ICollection<Doctor> ReadAll();
    bool Update(int id, Doctor doctor);
    bool Delete(int id);
}
