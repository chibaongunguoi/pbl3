using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace REPO.Controllers;

[Authorize]
public class UserController : BaseController
{
    private readonly ILogger<UserController> _logger;

    public UserController(ILogger<UserController> logger)
    {
        _logger = logger;
    }

    [Authorize(Roles = UserRole.Teacher)]
    public IActionResult Profile()
    {
        string username = User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
        return View(new TeacherProfileEditForm(username));
    }

    [Authorize]
    public IActionResult ChangePassword()
    {
        return View();
    }
}
