using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace REPO.Controllers;

public class RatingController : BaseController
{
    // [Authorize]
    public IActionResult Manage()
    {
        // var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        // if (userId == null)
        //     return Redirect("/Auth/Login");

        // // Get ratings for the user
        // RatingPage page = new(int.Parse(userId));
        // ViewBag.page = page;
        return View();
    }

    // [Authorize]
    // [HttpPost]
    // public IActionResult Submit(RatingForm form)
    // {
    //     var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    //     var userName = User.FindFirst(ClaimTypes.Name)?.Value;
    //     if (userId == null)
    //         return Redirect("/Auth/Login");

    //     // Set the student ID from claims
    //     form.student_id = int.Parse(userId);

    //     var log = form.execute();
    //     if (!log.success)
    //     {
    //         // Handle validation errors
    //         return RedirectToAction("Manage");
    //     }

    //     return Redirect($"/Course/Detail?course_id={form.course_id}");
    // }

    [HttpPost]
    public IActionResult SubmitRating(int semesterId, int stars, string comment)
    {
        string? username = User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;

        Query q = new(Tbl.student);
        q.Where(Field.student__username, username);
        q.Output(Field.student__id);
        int? stuId = null;
        QDatabase.Exec(conn => q.Select(conn, reader => stuId = QDataReader.GetInt(reader)));

        if (stuId is null)
        {
            TempData["ErrorMessage"] = "Không thể gửi đánh giá.";
            return Redirect("/Student/Course");
        }

        q = new(Tbl.rating);
        q.Where(Field.rating__stu_id, stuId);
        q.Where(Field.rating__semester_id, semesterId);
        int count = 0;
        QDatabase.Exec(conn => count = q.Count(conn));
        if (count > 0)
        {
            Query updateQ = new(Tbl.rating);
            updateQ.Set(Field.rating__stars, stars);
            updateQ.SetNString(Field.rating__description, comment);
            updateQ.Where(Field.rating__stu_id, stuId);
            updateQ.Where(Field.rating__semester_id, semesterId);
            QDatabase.Exec(updateQ.Update);
            TempData["SuccessMessage"] = "Đánh giá đã được cập nhật!";
        }
        else
        {
            Rating rating = new()
            {
                StuId = stuId ?? 0,
                SemesterId = semesterId,
                Stars = stars,
                Timestamp = DateTime.Now,
                Description = comment
            };
            Query insertQ = new(Tbl.rating);
            QDatabase.Exec(conn => insertQ.Insert(conn, rating));
            TempData["SuccessMessage"] = "Đánh giá thành công!";
        }
        return Redirect("/Student/Course");
    }
}