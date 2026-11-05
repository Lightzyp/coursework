using DMLab3CRRUDM2M.Services;
using Microsoft.AspNetCore.Mvc;

namespace DMLab3CRRUDM2M.Controllers;

public class PatientsController : Controller
{
    private readonly IPatientRepository _patients;

    public PatientsController(IPatientRepository patients)
    {
        _patients = patients;
    }

    public IActionResult Index()
    {
        return View(_patients.ReadAll());
    }

    public IActionResult Details(int id)
    {
        var patient = _patients.Read(id);
        if (patient == null)
        {
            return NotFound();
        }

        return View(patient);
    }
}
