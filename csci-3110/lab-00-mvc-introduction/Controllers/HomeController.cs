using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using csci3110LAB00DM.Models;

namespace csci3110LAB00DM.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
     
    public IActionResult MyBook()
    {
        return View();
    }

     public IActionResult TimesTable()
    {
        return View();
    }



    [HttpPost]

     public IActionResult Data(string userName)
     {
        ViewData["Username"] = userName;
        return View();
     }

     public IActionResult SimpleTypes()
     {
        return View();
     }


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
