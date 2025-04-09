using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using REPO.Models;

namespace REPO.Controllers;

public class SemesterController : Controller
{
     public IActionResult Add()
    {
        return View();
    }
   
}
