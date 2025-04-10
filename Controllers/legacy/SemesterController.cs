using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using REPO.Models;

namespace REPO.Controllers;

public class SemesterController : Controller
{
    public IActionResult Add()
    {
        string? user_role = HttpContext.Session.GetString(SessionKey.user_role);
        int? tch_id = null;
        switch (user_role)
        {
            case SessionRole.teacher:
                tch_id = HttpContext.Session.GetInt32(SessionKey.user_id);
                break;
            case SessionRole.admin:
                // TODO:
                tch_id = 2001;
                break;
        }
        if (tch_id is null)
        {
            return Redirect("/Home");
        }
        AddSemesterPage page = new(tch_id ?? 0);
        ViewBag.page = page;
        return View();
    }

    [HttpPost]
    public IActionResult add_semester(AddSemesterForm form)
    {
        form.print_log();
        string? user_role = HttpContext.Session.GetString(SessionKey.user_role);
        int? tch_id = null;
        switch (user_role)
        {
            case SessionRole.teacher:
                tch_id = HttpContext.Session.GetInt32(SessionKey.user_id);
                break;
            case SessionRole.admin:
                // TODO:
                tch_id = 2001;
                break;
        }
        if (tch_id is null)
            return RedirectToAction("Add");
        AddSemesterFormLog log = form.execute();
        if (!log.success)
            return RedirectToAction("Add");

        return Redirect($"/Course/Detail?course_id={form.course_id}");
    }
}
