using System.Diagnostics;
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
        return View();
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
        QDatabase.Exec(conn => form.Execute(conn, ModelState, username, out course, out semester));

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
        QDatabase.Exec(conn => form.Execute(conn, ModelState, out semester));

        if (semester is null)
        {
            return View(form);
        }
        return Redirect($"~/Course/Detail?courseId={semester.CourseId}");
    }
}