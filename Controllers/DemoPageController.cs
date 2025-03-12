namespace REPO.Controllers;

using Microsoft.AspNetCore.Mvc;

public class DemoPageController : BaseController
{
    // ========================================================================
    public IActionResult Index()
    {
        List<Teacher> teachers = Database.exec_list(conn =>
            new Query(Table.teacher).select<Teacher>(conn)
        );
        ViewBag.teachers = teachers;
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
