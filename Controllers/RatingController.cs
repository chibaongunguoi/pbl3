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
}

/* EOF */
