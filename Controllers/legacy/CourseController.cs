using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
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
        string? course_id_ = Request.Query["course_id"];
        int course_id = -1;
        if (course_id_ is not null)
        {
            course_id = int.Parse(course_id_);
        }
        DetailedCoursePage course_page = new(course_id);
        if (course_page.invalid)
            return Redirect("Index");
        ViewBag.teacher = course_page.teachers[0];
        ViewBag.course = course_page.courses[0];
        return View();
    }

    public IActionResult Manage()
    {
        return View();
    }

    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    public IActionResult add_course(AddCourseForm form)
    {
        int? role = HttpContext.Session.GetInt32(SessionKey.role);
        int? tch_id = null;
        switch (role)
        {
            case (int)SessionRole.teacher:
                tch_id = HttpContext.Session.GetInt32(SessionKey.user_id);
                break;
            case (int)SessionRole.admin:
                // TODO:
                tch_id = 2001;
                break;
        }
        if (tch_id is null)
            return RedirectToAction("Add");
        AddCourseFormLog log = form.execute(tch_id ?? 0);
        if (!log.success)
            return RedirectToAction("Add");

        return RedirectToAction("Index");
    }

    public IActionResult Privacy()
    {
        StringValues course_name_;
        Request.Form.TryGetValue("course_name", out course_name_);
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
