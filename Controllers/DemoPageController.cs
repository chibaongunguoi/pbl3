namespace REPO.Controllers;

using Microsoft.AspNetCore.Mvc;

public class DemoPageController : BaseController
{
    // ========================================================================
    public IActionResult Index()
    {
        ViewData["value"] = 123;
        return View();
    }

    // ========================================================================
}

/* EOF */
