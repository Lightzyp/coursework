using DMLab3CRRUDM2M.Models.Entities;

namespace DMLab3CRRUDM2M.Services;

public interface IPatientRepository
{
    Patient Create(Patient patient);
    Patient? Read(int id);
    ICollection<Patient> ReadAll();
    bool Update(int id, Patient patient);
    bool Delete(int id);
}
