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

    // ------------------------------------------------------------------------
    public IActionResult Teachers()
    {
        List<Teacher> teachers = Database.exec_list(conn =>
            new Query(Table.teacher).select<Teacher>(conn)
        );
        return View();
    }

    // ========================================================================
}

/* EOF */
