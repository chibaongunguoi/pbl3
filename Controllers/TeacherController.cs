using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace REPO.Controllers;

public class TeacherController : BaseController
{
    private readonly ILogger<TeacherController> _logger;

    public TeacherController(ILogger<TeacherController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return Redirect("/");
    }

    public IActionResult Profile(int tchId, BriefCourseFilter filter)
    {
        return View(new DetailedTeacherPage(tchId, filter));
    }

    [Authorize(Roles = "Teacher")]
    public IActionResult EditProfile()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Redirect("/Auth/Login");
            
        int teacherId = int.Parse(userId);
        // DetailedTeacherPage page = new(teacherId);
        // ViewBag.page = page;
        return View();
    }
}
