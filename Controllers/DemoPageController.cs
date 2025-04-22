using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace REPO.Controllers;

[Authorize(Roles = "Admin")]
public class DemoPageController : BaseController
{
    public IActionResult Index()
    {
        ViewBag.lmao = 123;
        return View();
    }

    public IActionResult Teachers()
    {
        return View();
    }
}

/* EOF */
