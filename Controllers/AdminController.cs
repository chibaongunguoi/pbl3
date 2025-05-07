using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace REPO.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : BaseController
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
    public IActionResult TeacherAdd()
    {
        return View();
    }

    public IActionResult Dashboard()
    {
        return View();
    }
}
