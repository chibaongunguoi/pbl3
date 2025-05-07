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

    public IActionResult Index(BriefCourseFilter filter)
    {
        return View(new BriefCoursePage(filter));
    }

    public IActionResult Detail(int courseId)
    {
        return View(new DetailedCoursePage(courseId));
    }

    [Authorize]
    public IActionResult Manage()
    {
        string role = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
        return role switch
        {
            UserRole.Student => Redirect("/Student/Course"),
            UserRole.Teacher => Redirect("/TeacherManage/ManageCourse"),
            _ => Redirect("/"),
        };
    }

    [Authorize(Roles = UserRole.Student)]
    public IActionResult Payment(int courseId)
    {
        int stuId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value is string id ? int.Parse(id) : 0;
        bool enrolled = false;
        QDatabase.Exec(conn => enrolled = CourseQuery.checkStudentEnrolled(conn, courseId, stuId));

        if (enrolled)
        {
            return RedirectToAction(nameof(Detail), new { courseId = courseId });
        }

        ViewBag.page = new CoursePaymentPage(courseId, stuId);
        return View();
    }

    [Authorize(Roles = UserRole.Student)]
    [HttpPost]
    public IActionResult SubmitPayment(int courseId, int semesterId, int stuId)
    {
        Request request = new()
        {
            StuId = stuId,
            SemesterId = semesterId,
            Timestamp = DateTime.Now,
            Status = RequestStatus.waiting
        };

        Query q = new(Tbl.request);
        QDatabase.Exec(conn =>
        {
            q.Insert(conn, request);
        });
        return RedirectToAction(nameof(Detail), new { courseId = courseId });
    }
}
