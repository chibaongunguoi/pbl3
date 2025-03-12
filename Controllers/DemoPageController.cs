namespace REPO.Controllers;

using Microsoft.AspNetCore.Mvc;

public class DemoPageController : BaseController
{
    // ========================================================================
    public IActionResult Index()
    {
        ViewBag.lmao = 123;
        return View();
    }

    // ------------------------------------------------------------------------
    public IActionResult Teachers()
    {
        return View();
    }

    // ========================================================================
}

/* EOF */
