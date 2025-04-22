using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace REPO.Controllers;

public class StudentController : BaseController
{
    // [Authorize(Roles = "Student")]
    // public IActionResult Request()
    // {
    //     var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    //     if (userId == null)
    //         return Redirect("/Auth/Login");

    //     return View();
    // }

    // [Authorize(Roles = "Student")]
    // [HttpPost]
    // public IActionResult SubmitRequest(StudentRequestForm form)
    // {
    //     var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    //     if (userId == null)
    //         return Redirect("/Auth/Login");

    //     form.student_id = int.Parse(userId);
    //     var log = form.execute();
    //     if (!log.success)
    //     {
    //         // Handle validation errors
    //         return RedirectToAction("Request");
    //     }

    //     return RedirectToAction("Request");
    // }
}
