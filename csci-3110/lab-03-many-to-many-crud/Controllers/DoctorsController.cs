using DMLab3CRRUDM2M.Services;
using Microsoft.AspNetCore.Mvc;

namespace DMLab3CRRUDM2M.Controllers;

public class DoctorsController : Controller
{
    private readonly IDoctorRepository _doctors;

    public DoctorsController(IDoctorRepository doctors)
    {
        _doctors = doctors;
    }

    public IActionResult Index()
    {
        return View(_doctors.ReadAll());
    }

    public IActionResult Details(int id)
    {
        var doctor = _doctors.Read(id);
        if (doctor == null)
        {
            return NotFound();
        }

        return View(doctor);
    }
}
