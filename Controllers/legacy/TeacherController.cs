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
        string? page_ = Request.Query["page"];
        int page = 1;
        if (page_ is not null)
        {
            page = int.Parse(page_);
        }
        BriefTeacherPage teacher_page = new(page);
        ViewBag.teacher = teacher_page.teachers[0];
        ViewBag.currentPage = page;
        ViewBag.maxIndexPage = teacher_page.total_num_pages;
        return View();
    }

    public IActionResult Profile()
    {
        List<BriefTeacherCard> teacher_dicts = Database.exec_list(conn =>
            BriefTeacherCard.get_page(conn, 1)
        );
        ViewBag.teachers = teacher_dicts;
        // List<BriefCourseCard> courses = new();
        // Database.exec(conn => courses = BriefCourseCard.get_page(conn, 1));
        // ViewBag.courses = courses;
        ViewBag.currentPage = 1;
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
