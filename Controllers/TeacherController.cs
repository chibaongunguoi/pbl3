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

    public IActionResult Profile()
    {
        int? tchId = null;
        
        // If user is logged in, check if they are viewing their own profile
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
        
        if (userRole == UserRole.Teacher && userId != null)
        {
            tchId = int.Parse(userId);
        }
        else
        {
            // Otherwise use the ID from query params for public profile viewing
            tchId = Session.getInt(Request.Query, UrlKey.tchId);
        }

        if (tchId is null)
            return RedirectToAction("Index");

        DetailedTeacherPage page = new(tchId.Value);
        ViewBag.page = page;
        ViewBag.isOwnProfile = userRole == UserRole.Teacher && userId != null && tchId == int.Parse(userId);
        return View();
    }

    [Authorize(Roles = "Teacher")]
    public IActionResult EditProfile()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Redirect("/Auth/Login");
            
        int teacherId = int.Parse(userId);
        DetailedTeacherPage page = new(teacherId);
        ViewBag.page = page;
        return View();
    }
}
