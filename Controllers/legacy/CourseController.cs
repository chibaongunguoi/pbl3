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
        ViewBag.maxIndexPage = course_page.total_num_pages;
        Console.WriteLine($"Fetched {course_page.courses.Count} courses");
        return View();
    }

    public IActionResult Detail()
    {
        string? course_id_ = Request.Query["course_id"];
        string? page_ = Request.Query["page"];
        int course_id = -1;
        int page = 1;
        try
        {
            course_id = int.Parse(course_id_ ?? "-1");
            page = int.Parse(page_ ?? "1");
        }
        catch (Exception)
        {
            return Redirect("Index");
        }

        DetailedCoursePage course_page = new(course_id, page);
        if (course_page.invalid)
        {
            return Redirect("Index");
        }
        ViewBag.page = course_page;
        ViewBag.currentPage = page;
        return View();
    }

    public IActionResult Manage()
    {
        string? user_role = HttpContext.Session.GetString(SessionKey.user_role);
        int? tch_id = null;
        switch (user_role)
        {
            case SessionRole.teacher:
                tch_id = HttpContext.Session.GetInt32(SessionKey.user_id);
                break;
            case SessionRole.admin:
                // TODO:
                tch_id = 2001;
                break;
        }
        if (tch_id is null)
            return RedirectToAction("Add");
        ManageCoursePage page = ManageCoursePage.get_by_tch_id(tch_id ?? 0);
        ViewBag.page = page;
        return View();
    }

    public IActionResult Add()
    {
        return View();
    }

    public IActionResult Payment()
    {
        string? course_id_ = Request.Query["course_id"];
        int course_id = 0;
        if (!int.TryParse(course_id_, out course_id))
            return Redirect("/");
        return View();
    }

    [HttpPost]
    public IActionResult add_course(AddCourseForm form)
    {
        string? user_role = HttpContext.Session.GetString(SessionKey.user_role);
        int? tch_id = null;
        switch (user_role)
        {
            case SessionRole.teacher:
                tch_id = HttpContext.Session.GetInt32(SessionKey.user_id);
                break;
            case SessionRole.admin:
                // TODO:
                tch_id = 2001;
                break;
        }
        if (tch_id is null)
            return RedirectToAction("Add");
        AddCourseForm.Log log = form.execute(tch_id ?? 0);
        if (!log.success)
            return RedirectToAction("Add");

        return Redirect($"Detail/?course_id={log.course_id}");
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
