using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace REPO.Controllers;

public class SemesterController : BaseController
{
    [Authorize(Roles="Teacher,Admin")]
    public IActionResult Add()
    {
        string username = User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty  ;
        AddSemesterPage page = new(username);
        ViewBag.page = page;
        return View();
    }

    [Authorize]
    public IActionResult Manage()
    {
        int? courseId = UrlQuery.getInt(Request.Query, UrlKey.courseId);
        if (courseId is null)
        {
            return RedirectToAction("Index", "Course");
        }

        SemesterPage page = new(courseId.Value);
        ViewBag.page = page;
        return View();
    }

    // [Authorize(Roles = "Teacher,Admin")]
    // [HttpPost]
    // public IActionResult add_semester(AddSemesterForm form)
    // {
        // form.print_log();
        // var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
        // var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        // int? tch_id = null;
        
        // switch (userRole)
        // {
        //     case UserRole.Teacher:
        //         tch_id = int.Parse(userId ?? "0");
        //         break;
        //     case UserRole.Admin:
        //         tch_id = AccountUtils.getAdminTeacherId();
        //         break;
        // }
        
        // if (tch_id is null)
        //     return RedirectToAction("Add");
        // AddSemesterFormLog log = form.execute();
        // if (!log.Success)
        //     return RedirectToAction("Add");

        // return Redirect($"/Course/Detail?course_id={form.CourseId}");
    // }
}
