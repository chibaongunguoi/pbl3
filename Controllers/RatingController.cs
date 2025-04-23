using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace REPO.Controllers;

public class RatingController : BaseController
{
    // [Authorize]
    // public IActionResult Manage()
    // {
    //     var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    //     if (userId == null)
    //         return Redirect("/Auth/Login");

    //     // Get ratings for the user
    //     RatingPage page = new(int.Parse(userId));
    //     ViewBag.page = page;
    //     return View();
    // }

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
        int stuId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        Query q = new(Tbl.rating);
        q.Where(Field.rating__semester_id, semesterId);
        q.Where(Field.rating__stu_id, stuId);
        int count = 0;
        QDatabase.exec(conn => count = q.Count(conn) ); 
        if (count > 0)
        {
            Query updateQ = new(Tbl.rating);
            updateQ.Set(Field.rating__stars, stars);
            updateQ.Set(Field.rating__description, comment);
            updateQ.Where(Field.rating__semester_id, semesterId);
            updateQ.Where(Field.rating__stu_id, stuId);
            QDatabase.exec(conn => updateQ.Update(conn));
        }
        else 
        {
            Rating rating = new ()
            {
                StuId = stuId,
                SemesterId = semesterId,
                Stars = stars,
                Timestamp = DateTime.Now,
                Description = comment
            };
            Query insertQ = new(Tbl.rating);
            QDatabase.exec(conn => insertQ.Insert(conn, rating));
        }
        return Redirect("/Student/Course");
    }
}