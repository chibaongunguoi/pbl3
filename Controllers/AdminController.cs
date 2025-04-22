using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using REPO.Models;

namespace REPO.Controllers;

public class AdminController : Controller
{
    private readonly ILogger<AdminController> _logger;

    public AdminController(ILogger<AdminController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    public IActionResult CourseManage()
    {
        return View();
    }
    public IActionResult SemesterManage()
    {
        return View();
    }
    public IActionResult StudentManage()
    {
        return View();
    }
    public IActionResult TeacherManage()
    {
        return View();
    }
    public IActionResult Dashboard()
    {
        return View();
    }


}
