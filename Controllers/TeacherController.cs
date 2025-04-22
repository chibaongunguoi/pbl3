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
        string? page_ = Request.Query[UrlKey.page];
        int page = 1;
        if (page_ is not null)
        {
            page = int.Parse(page_);
        }
        BriefTeacherPage teacher_page = new(page);
        ViewBag.teachers = teacher_page.teachers;
        ViewBag.currentPage = page;
        ViewBag.maxIndexPage = teacher_page.total_num_pages;
        return View();
    }

    public IActionResult Profile()
    {
        int? tchId = Session.getInt(Request.Query, UrlKey.tchId);
        if (tchId is null)
            return Redirect("Index");
        DetailedTeacherPage page = new(tchId.Value);
        ViewBag.page = page;
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
