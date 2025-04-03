using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using REPO.Models;

namespace REPO.Controllers;

public class CourseController : Controller
{
    private readonly ILogger<CourseController> _logger;

    public CourseController(ILogger<CourseController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        List<BriefCourseCard> courses = new();
        Database.exec(conn => courses = Test2.get_brief_course_cards(conn));
        ViewBag.courses = courses;
        Console.WriteLine($"Fetched {courses.Count} courses");
        return View();
    }

    public IActionResult Detail()
    {
        List<BriefTeacherCard> teacher_dicts = Database.exec_list(conn => Test2.demo2(conn));
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
