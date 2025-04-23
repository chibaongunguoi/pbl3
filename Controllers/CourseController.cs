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
        ViewBag.page = new BriefCoursePage();
        return View();
    }

    public IActionResult Detail(int courseId)
    {
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
                isOwner = q.Count(conn) > 0;
            });
        }

        DetailedCoursePage page = new(courseId);
        ViewBag.page = page;
        ViewBag.isOwner = isOwner;
        return View();
    }

    [Authorize]
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
                tch_id = 2001;
                break;
        }
        switch (userRole)
        {
            case UserRole.Teacher or UserRole.Admin:
                ManageCoursePage page = ManageCoursePage.get_by_tch_id(tch_id ?? 0);
                ViewBag.page = page;
                return View();

            case UserRole.Student:
                return Redirect("/Student/Course");
        }

        return Redirect("/");
    }

    [Authorize(Roles = "Teacher,Admin")]
    public IActionResult Add()
    {
        return View();
    }

    [Authorize(Roles = UserRole.Student)]
    public IActionResult Payment(int course_id)
    {
        int stuId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value is string id ? int.Parse(id) : 0;
        bool enrolled = false;
        QDatabase.exec(conn => enrolled = CourseQuery.checkStudentEnrolled(conn, course_id, stuId));

        if (enrolled)
        {
            return RedirectToAction(nameof(Detail), new { courseId = course_id });
        }

        ViewBag.page = new CoursePaymentPage(course_id, stuId);
        return View();
    }

    [Authorize(Roles = UserRole.Student)]
    [HttpPost]
    public IActionResult Payment(int courseId, int semesterId, int stuId)
    {
        Request request = new()
        {
            StuId = stuId,
            SemesterId = semesterId,
            Timestamp = DateTime.Now,
            Status = RequestStatus.waiting
        };

        Query q = new(Tbl.request);
        QDatabase.exec(conn =>
        {
            q.Insert(conn, request);
        });
        return View(nameof(Detail), new { courseId = courseId });
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
}
