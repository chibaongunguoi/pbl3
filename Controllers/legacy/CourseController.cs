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
        string? page_ = Request.Query["page"];
        int page = 1;
        if (page_ is not null)
        {
            page = int.Parse(page_);
        }
        BriefCoursePage course_page = new(page);
        ViewBag.courses = course_page.courses;
        ViewBag.currentPage = page;
        ViewBag.maxIndexPage = course_page.total_num_pages;
        Console.WriteLine($"Fetched {course_page.courses.Count} courses");
        return View();
    }

    public IActionResult Detail()
    {
        List<BriefTeacherCard> teacher_dicts = Database.exec_list(conn =>
            BriefTeacherCard.get_page(conn)
        );
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
