namespace REPO.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using module_demo;
using module_query;
public class AuthController : BaseController
{
    // ========================================================================

    public IActionResult Login()
    {
        var user = DemoUserQuery.get_all_demo_users();
        var user1001 = DemoUserQuery.get_demo_user_by_id(1002);
        ViewBag.oneuser = user1001.IsNullOrEmpty() ? "Empty" : user1001[0].get_repr();
        return View("Login"); // Returns the view "Views/Auth/Login.cshtml"
    }
    public IActionResult Listuser()
    {
        var user = DemoUserQuery.get_all_demo_users();
        ViewBag.user = user;
        return View("ListUser"); // Returns the view "Views/Auth/Login.cshtml"
    }
    // [HttpPost]
    public IActionResult store()
    {
        string username = Request.Form["username"];
        string password = Request.Form["password"];
        ViewBag.username = username;
        // var user = DemoUserQuery.get_all_demo_users();
        var user1001 = DemoUserQuery.get_demo_user_by_id(int.Parse(username));
        ViewBag.oneuser = user1001.IsNullOrEmpty() ? "Empty" : user1001[0].get_repr();
        return View("user"); // Returns the view "Views/Auth/Login.cshtml"
    }

    // ========================================================================
}

/* EOF */
