using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using REPO.Models;

namespace REPO.Controllers;

public class TeacherController : Controller
{
    private readonly ILogger<TeacherController> _logger;

    public TeacherController(ILogger<TeacherController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        List<BriefTeacherCard> teacher_dicts = Database.exec_list(conn => Test2.demo2(conn));
        ViewBag.teachers = teacher_dicts;
        Console.WriteLine($"Fetched {teacher_dicts.Count} teachers");
        return View();
    }
    public IActionResult Profile()
    {
          List<Dictionary<string, string>> teacher_dicts = new();
        Database.exec(conn => teacher_dicts = Test2.demo(conn));
        ViewBag.teachers = teacher_dicts;
        Console.WriteLine($"Fetched {teacher_dicts.Count} teachers");
        return View();
    }
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(
            new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }
        );
    }
}
