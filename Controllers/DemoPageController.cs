namespace REPO.Controllers;

using Microsoft.AspNetCore.Mvc;

public class DemoPageController : BaseController
{
    public IActionResult Index()
    {
        return View();
    }
}
