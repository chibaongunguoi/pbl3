using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace REPO.Controllers;

[Authorize(Roles = UserRole.Student)]
public class StudentController : BaseController
{
    public IActionResult Course()
    {
        return View();
    }

    public IActionResult ManageProfile()
    {
        string username = User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
        return View(new StudentProfileForm(username));
    }

    [HttpPost]
    public IActionResult ManageProfile(StudentProfileForm form)
    {
        if (!ModelState.IsValid)
        {
            return View(form);
        }

        string username = User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
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


    public IActionResult CoursePayment(int courseId)
    {
        string username = User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
        CoursePaymentModel payment = new();
        payment.Init(courseId, username);
        TempData["ErrorMessage"] = payment.ErrorMessage;
        return View(payment);
    }

    [HttpPost]
    public IActionResult CoursePaymentPost([FromBody] CoursePaymentModel payment)
    {
        string username = User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
        payment.Init(payment.courseId, username);
        if (!payment.IsValid)
        {
            return Json(new { redirectUrl = Url.Action(nameof(CoursePayment), new { courseId = payment.courseId }) });
        }
        Request request = new()
        {
            StuId = payment.stuId,
            SemesterId = payment.semesterId,
            Timestamp = DateTime.Now,
            Status = RequestStatus.waiting
        };

        Query q = new(Tbl.request);
        QDatabase.Exec(conn =>
        {
            q.Insert(conn, request);
        });
        TempData["SuccessMessage"] = "Đăng kí học thành công!";
        return Json(new { redirectUrl = Url.Action(nameof(Course)) });
    }
}