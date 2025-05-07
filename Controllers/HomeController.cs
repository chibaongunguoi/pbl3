using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace REPO.Controllers;

public class HomeController : BaseController
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        if (User.IsInRole(UserRole.Admin))
        {
            return Redirect("/Admin/Dashboard");
        }
        return View(new BriefTeacherPage());
    }
}
