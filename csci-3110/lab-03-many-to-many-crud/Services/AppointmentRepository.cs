using DMLab3CRRUDM2M.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DMLab3CRRUDM2M.Services;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly ApplicationDbContext _db;

    public AppointmentRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public Appointment Create(Appointment appointment)
    {
        _db.Appointments.Add(appointment);
        _db.SaveChanges();
        return appointment;
    }

    public Appointment? Read(int id)
    {
        // Load both ends so forms can show the doctor's and patient's names.
        return _db.Appointments
            .Include(a => a.Doctor)
            .Include(a => a.Patient)
            .FirstOrDefault(a => a.Id == id);
    }

    public ICollection<Appointment> ReadAll()
    {
        return _db.Appointments
            .Include(a => a.Doctor)
            .Include(a => a.Patient)
            .OrderBy(a => a.Date)
            .ToList();
    }

    public bool Update(int id, Appointment appointment)
    {
        var existing = _db.Appointments.Find(id);
        if (existing == null)
        {
            return false;
        }

        // Only the appointment's own data changes; DoctorId and PatientId stay as they were.
        existing.Date = appointment.Date;
        existing.Notes = appointment.Notes;
        _db.SaveChanges();
        return true;
    }

    public bool Delete(int id)
    {
        var existing = _db.Appointments.Find(id);
        if (existing == null)
        {
            return false;
        }

        // Removes only this join row; the doctor and the patient are kept.
        _db.Appointments.Remove(existing);
        _db.SaveChanges();
        return true;
    }
}
