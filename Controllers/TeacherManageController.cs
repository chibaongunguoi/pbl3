using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace REPO.Controllers;

[Authorize(Roles = UserRole.Teacher)]
public class TeacherManageController : BaseController
{
    public IActionResult ManageCourse()
    {
        return View();
    }

    public IActionResult AddCourse()
    {
        return View(new AddCourseForm());
    }

    [HttpPost]
    public IActionResult AddCourse(AddCourseForm form)
    {
        if (!ModelState.IsValid)
        {
            return View(form);
        }

        string username = User.FindFirst(ClaimTypes.Name)?.Value ?? "";
        Course? course = null;
        Semester? semester = null;
        QDatabase.Exec(conn => form.Execute(conn, username, out course, out semester));

        if (course is null || semester is null)
        {
            return View(form);
        }
        return Redirect($"~/Course/Detail?courseId={semester.CourseId}");
    }

    public IActionResult AddSemester()
    {
        string username = User.FindFirst(ClaimTypes.Name)?.Value ?? "";
        return View(new AddSemesterForm(username));
    }

    [HttpPost]
    public IActionResult AddSemester(AddSemesterForm form)
    {
        if (!ModelState.IsValid)
        {
            return View(form);
        }

        Semester? semester = null;
        QDatabase.Exec(conn => form.Execute(conn, out semester));

        if (semester is null)
        {
            return View(form);
        }
        return Redirect($"~/Course/Detail?courseId={semester.CourseId}");
    }

    public IActionResult ManageSemester(int courseId)
    {
        return View(new ManageSemesterPage(courseId));
    }

    public IActionResult ManageRequest()
    {
        return View();
    }

    public IActionResult ManageRating()
    {
        return View();
    }

    public IActionResult ManageProfile()
    {
        string username = User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
        return View(new TeacherProfileEditForm(username));
    }

    [HttpPost]
    public IActionResult ManageProfile(TeacherProfileEditForm form)
    {
        string username = User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
        QDatabase.Exec(conn => form.Reset(conn, username));
        if (!ModelState.IsValid)
        {
            return View(form);
        }
        Account? account = null;
        QDatabase.Exec(conn => form.Execute(conn, username, TempData, out account));
        return View(form);
    }

    public IActionResult ChangePassword()
    {
        return View();
    }

    [HttpPost]
    public IActionResult ChangePassword(PasswordChangeForm form)
    {
        if (!ModelState.IsValid)
        {
            return View(form);
        }

        string username = User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
        string role = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;

        Account? account = null;
        QDatabase.Exec(conn => form.Execute(conn, username, role, TempData, out account));
        return View(form);
    }
}