using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using REPO.Models;

namespace REPO.Controllers;

public class StudentController : Controller
{
     public IActionResult Request()
    {
        return View();
    }
   
}
