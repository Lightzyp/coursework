using DMLab3CRRUDM2M.Models.Entities;
using DMLab3CRRUDM2M.Models.ViewModels;
using DMLab3CRRUDM2M.Services;
using Microsoft.AspNetCore.Mvc;

namespace DMLab3CRRUDM2M.Controllers;

public class AppointmentController : Controller
{
    private const string FromDoctor = "Doctor";
    private const string FromPatient = "Patient";

    private readonly IAppointmentRepository _appointments;
    private readonly IDoctorRepository _doctors;
    private readonly IPatientRepository _patients;

    public AppointmentController(
        IAppointmentRepository appointments,
        IDoctorRepository doctors,
        IPatientRepository patients)
    {
        _appointments = appointments;
        _doctors = doctors;
        _patients = patients;
    }

    // ---------- Part 4: Create ----------

    // GET /Appointment/CreateForDoctor/{doctorId}
    public IActionResult CreateForDoctor(int id)
    {
        var doctor = _doctors.Read(id);
        if (doctor == null)
        {
            return NotFound();
        }

        return View(new CreateForDoctorViewModel { Doctor = doctor, Patients = _patients.ReadAll() });
    }

    // GET /Appointment/CreateForPatient/{patientId}
    public IActionResult CreateForPatient(int id)
    {
        var patient = _patients.Read(id);
        if (patient == null)
        {
            return NotFound();
        }

        return View(new CreateForPatientViewModel { Patient = patient, Doctors = _doctors.ReadAll() });
    }

    // Both setup URLs use the SAME order: /Appointment/SetupForXxx/{doctorId}/{patientId}.
    // So in both actions "id" is the doctor's id and "patientId" is the patient's id.

    [HttpGet]
    public IActionResult SetupForDoctor(int id, int patientId)
    {
        return ShowSetup(id, patientId, FromDoctor);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SetupForDoctor(int id, int patientId, AppointmentFormViewModel form)
    {
        return SaveSetup(id, patientId, FromDoctor, form);
    }

    [HttpGet]
    public IActionResult SetupForPatient(int id, int patientId)
    {
        return ShowSetup(id, patientId, FromPatient);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SetupForPatient(int id, int patientId, AppointmentFormViewModel form)
    {
        return SaveSetup(id, patientId, FromPatient, form);
    }

    private IActionResult ShowSetup(int doctorId, int patientId, string from)
    {
        var form = new AppointmentFormViewModel();
        if (!FillNames(form, doctorId, patientId, from))
        {
            return NotFound();
        }

        return View("Setup", form);
    }

    private IActionResult SaveSetup(int doctorId, int patientId, string from, AppointmentFormViewModel form)
    {
        // Both ends must exist before we link them.
        if (!FillNames(form, doctorId, patientId, from))
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View("Setup", form);
        }

        _appointments.Create(new Appointment
        {
            DoctorId = doctorId,
            PatientId = patientId,
            Date = form.Date!.Value,
            Notes = form.Notes
        });

        // Return to the index page the user started from.
        return from == FromDoctor
            ? RedirectToAction("Index", "Doctors")
            : RedirectToAction("Index", "Patients");
    }

    // Looks up both ends and copies their names into the form. Returns false if either is missing.
    private bool FillNames(AppointmentFormViewModel form, int doctorId, int patientId, string from)
    {
        var doctor = _doctors.Read(doctorId);
        var patient = _patients.Read(patientId);
        if (doctor == null || patient == null)
        {
            return false;
        }

        form.DoctorId = doctor.Id;
        form.DoctorName = doctor.Name;
        form.PatientId = patient.Id;
        form.PatientName = patient.Name;
        form.From = from;
        return true;
    }

    // ---------- Part 6: Edit ----------

    // GET /Appointment/Edit/{appointmentId}?from=Doctor
    [HttpGet]
    public IActionResult Edit(int id, string? from)
    {
        if (!IsValidOrigin(from))
        {
            return BadRequest();
        }

        var appointment = _appointments.Read(id);
        if (appointment == null)
        {
            return NotFound();
        }

        var form = new AppointmentFormViewModel { Date = appointment.Date, Notes = appointment.Notes };
        FillFromAppointment(form, appointment, from!);
        return View(form);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, string? from, AppointmentFormViewModel form)
    {
        if (!IsValidOrigin(from))
        {
            return BadRequest();
        }

        var appointment = _appointments.Read(id);
        if (appointment == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            FillFromAppointment(form, appointment, from!);
            return View(form);
        }

        // Only Date and Notes change; the doctor and patient stay the same.
        _appointments.Update(id, new Appointment { Date = form.Date!.Value, Notes = form.Notes });
        return BackToDetails(appointment, from!);
    }

    // ---------- Part 7: Delete ----------

    // GET /Appointment/Delete/{appointmentId}?from=Patient  (confirmation page only, changes nothing)
    [HttpGet]
    public IActionResult Delete(int id, string? from)
    {
        if (!IsValidOrigin(from))
        {
            return BadRequest();
        }

        var appointment = _appointments.Read(id);
        if (appointment == null)
        {
            return NotFound();
        }

        ViewData["From"] = from;
        return View(appointment);
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id, string? from)
    {
        if (!IsValidOrigin(from))
        {
            return BadRequest();
        }

        var appointment = _appointments.Read(id);
        if (appointment == null)
        {
            return NotFound();
        }

        // Removes only the appointment (the link); the doctor and patient remain.
        _appointments.Delete(id);
        return BackToDetails(appointment, from!);
    }

    // ---------- Helpers ----------

    private static bool IsValidOrigin(string? from)
    {
        return from == FromDoctor || from == FromPatient;
    }

    private static void FillFromAppointment(AppointmentFormViewModel form, Appointment appointment, string from)
    {
        form.AppointmentId = appointment.Id;
        form.DoctorId = appointment.DoctorId;
        form.DoctorName = appointment.Doctor.Name;
        form.PatientId = appointment.PatientId;
        form.PatientName = appointment.Patient.Name;
        form.From = from;
    }

    // Return to the Details page the user started from. The id comes from the stored
    // appointment, never from the form, so a user cannot redirect somewhere else.
    private IActionResult BackToDetails(Appointment appointment, string from)
    {
        return from == FromDoctor
            ? RedirectToAction("Details", "Doctors", new { id = appointment.DoctorId })
            : RedirectToAction("Details", "Patients", new { id = appointment.PatientId });
    }
}
