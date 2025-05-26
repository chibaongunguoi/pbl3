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
        string username = User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
        QDatabase.Exec(conn=> form.Reset(conn, username));
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
        });        TempData["SuccessMessage"] = "Đăng kí học thành công!";
        return Json(new { redirectUrl = Url.Action(nameof(Course)) });
    }

    [HttpGet]
    public IActionResult Notifications()
    {
        int studentId = GetCurrentStudentId();
        List<Notification> notifications = new();
        QDatabase.Exec(conn =>
        {
            Query q = new(Tbl.notification);
            q.Where(Field.notification__stu_id, studentId);
            q.OrderBy(Field.notification__timestamp, desc: true);
            q.Output(Field.notification__id);
            q.Output(Field.notification__message);
            q.Output(Field.notification__timestamp);
            q.Output(Field.notification__is_read);
            q.Select(conn, reader =>
            {
                int pos = 0;
                Notification n = new ()
                {
                    Id = QDataReader.GetInt(reader, ref pos),
                    StudentId = studentId,
                    Message = QDataReader.GetString(reader, ref pos),
                    CreatedAt = QDataReader.GetDateTime(reader, ref pos),
                    IsRead = QDataReader.GetInt(reader, ref pos)
                };
                notifications.Add(n);
            });
        });
        return View("~/Views/Student/Notifications.cshtml", notifications);
    }

    [HttpPost]
    public IActionResult MarkNotificationRead(int id)
    {
        int studentId = GetCurrentStudentId();
        QDatabase.Exec(conn =>
        {
            Query q = new(Tbl.notification);
            q.Set(Field.notification__is_read, 1);
            q.Where(Field.notification__stu_id, studentId);
            q.Where(Field.notification__id, id);
            q.Update(conn);
        });
        return RedirectToAction("Notifications");
    }    private int GetCurrentStudentId()
    {
        // Get the username from the authentication claims
        string username = User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
        if (string.IsNullOrEmpty(username))
            return 0;
        
        // Query the database to get the student ID from the username
        int studentId = 0;
        QDatabase.Exec(conn =>
        {
            Query q = new(Tbl.student);
            q.Where(Field.student__username, username);
            q.Output(Field.student__id);
            q.Select(conn, reader =>
            {
                studentId = QDataReader.GetInt(reader);
            });
        });
        
        return studentId;
    }
}