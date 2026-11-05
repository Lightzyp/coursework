using DMLab3CRRUDM2M.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DMLab3CRRUDM2M.Services;

public class PatientRepository : IPatientRepository
{
    private readonly ApplicationDbContext _db;

    public PatientRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public Patient Create(Patient patient)
    {
        _db.Patients.Add(patient);
        _db.SaveChanges();
        return patient;
    }

    public Patient? Read(int id)
    {
        // Eager loading: bring in each appointment AND the doctor on the other end of it.
        return _db.Patients
            .Include(p => p.Appointments)
                .ThenInclude(a => a.Doctor)
            .FirstOrDefault(p => p.Id == id);
    }

    public ICollection<Patient> ReadAll()
    {
        // Appointments are loaded so the Index page can show a count for each patient.
        return _db.Patients
            .Include(p => p.Appointments)
            .OrderBy(p => p.Name)
            .ToList();
    }

    public bool Update(int id, Patient patient)
    {
        var existing = _db.Patients.Find(id);
        if (existing == null)
        {
            return false;
        }

        existing.Name = patient.Name;
        _db.SaveChanges();
        return true;
    }

    public bool Delete(int id)
    {
        var existing = _db.Patients.Find(id);
        if (existing == null)
        {
            return false;
        }

        _db.Patients.Remove(existing);
        _db.SaveChanges();
        return true;
    }
}
