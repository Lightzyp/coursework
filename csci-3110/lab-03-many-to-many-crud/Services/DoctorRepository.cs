using DMLab3CRRUDM2M.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DMLab3CRRUDM2M.Services;

public class DoctorRepository : IDoctorRepository
{
    private readonly ApplicationDbContext _db;

    public DoctorRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public Doctor Create(Doctor doctor)
    {
        _db.Doctors.Add(doctor);
        _db.SaveChanges();
        return doctor;
    }

    public Doctor? Read(int id)
    {
        // Eager loading: bring in each appointment AND the patient on the other end of it.
        return _db.Doctors
            .Include(d => d.Appointments)
                .ThenInclude(a => a.Patient)
            .FirstOrDefault(d => d.Id == id);
    }

    public ICollection<Doctor> ReadAll()
    {
        // Appointments are loaded so the Index page can show a count for each doctor.
        return _db.Doctors
            .Include(d => d.Appointments)
            .OrderBy(d => d.Name)
            .ToList();
    }

    public bool Update(int id, Doctor doctor)
    {
        var existing = _db.Doctors.Find(id);
        if (existing == null)
        {
            return false;
        }

        existing.Name = doctor.Name;
        existing.Specialty = doctor.Specialty;
        _db.SaveChanges();
        return true;
    }

    public bool Delete(int id)
    {
        var existing = _db.Doctors.Find(id);
        if (existing == null)
        {
            return false;
        }

        _db.Doctors.Remove(existing);
        _db.SaveChanges();
        return true;
    }
}
