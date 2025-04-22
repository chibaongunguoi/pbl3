using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace REPO.Controllers;

public class CourseController : BaseController
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
        ViewBag.page = new BriefCoursePage(page);
        return View();
    }

    public IActionResult Detail()
    {
        int? courseId = Session.getInt(Request.Query, UrlKey.courseId);
        if (courseId is null)
            return RedirectToAction("Index");

        // If user is logged in, check if they own/manage this course
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        bool isOwner = false;
        if (userRole is UserRole.Teacher or UserRole.Admin)
        {
            int? teacherId = null;
            
            if (userRole == UserRole.Teacher)
            {
                teacherId = int.Parse(userId ?? "0");
            }
            else if (userRole == UserRole.Admin)
            {
                teacherId = AccountUtils.getAdminTeacherId();
            }

            // Verify if course belongs to this teacher
            QDatabase.exec(conn =>
            {
                Query q = new(Tbl.course);
                q.Where(Field.course__id, courseId);
                q.Where(Field.teacher__id, teacherId);
                isOwner = q.count(conn) > 0;
            });
        }

        DetailedCoursePage page = new(courseId.Value);
        ViewBag.page = page;
        ViewBag.isOwner = isOwner;
        return View();
    }

    [Authorize(Roles = "Teacher,Admin")]
    public IActionResult Manage()
    {
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        int? tch_id = null;
        
        switch (userRole)
        {
            case UserRole.Teacher:
                tch_id = int.Parse(userId ?? "0");
                break;
            case UserRole.Admin:
                tch_id = AccountUtils.getAdminTeacherId();
                break;
        }
        
        ManageCoursePage page = ManageCoursePage.get_by_tch_id(tch_id ?? 0);
        ViewBag.page = page;
        return View();
    }

    [Authorize(Roles = "Teacher,Admin")]
    public IActionResult Add()
    {
        return View();
    }

    [Authorize]
    public IActionResult Payment()
    {
        string? courseId_ = Request.Query["course_id"];
        int courseId = 0;
        if (!int.TryParse(courseId_, out courseId))
            return Redirect("/");
            
        // Verify course exists
        bool courseExists = false;
        QDatabase.exec(conn =>
        {
            Query q = new(Tbl.course);
            q.Where(Field.course__id, courseId);
            courseExists = q.count(conn) > 0;
        });
        
        if (!courseExists)
            return RedirectToAction("Index");

        ViewBag.courseId = courseId;
        return View();
    }

    [Authorize(Roles = "Teacher,Admin")]
    [HttpPost]
    public IActionResult add_course(AddCourseForm form)
    {
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        int? tch_id = null;
        
        switch (userRole)
        {
            case UserRole.Teacher:
                tch_id = int.Parse(userId ?? "0");
                break;
            case UserRole.Admin:
                tch_id = AccountUtils.getAdminTeacherId();
                break;
        }
            
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
}
