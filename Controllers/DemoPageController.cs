namespace REPO.Controllers;

using Microsoft.AspNetCore.Mvc;

public class DemoPageController : BaseController
{
    // ========================================================================
    public IActionResult Index()
    {
        List<int> a = Demo.demo_int();
        ViewData["value"] = a;
        return View();
    }

    // ========================================================================
}

/* EOF */
