namespace REPO.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using module_query;

public class AuthController : BaseController
{
    // ========================================================================

    public IActionResult Login()
    {
        var user = DemoUserQuery.get_all_demo_users();
        var user1001 = DemoUserQuery.get_demo_user_by_id(1002);
        ViewBag.oneuser = user1001.IsNullOrEmpty() ? "Empty" : user1001[0].ToString();
        return View("Login");
    }

    public IActionResult SignUp()
    {
        return View("SignUp");
    }

    public IActionResult Listuser()
    {
        var user = DemoUserQuery.get_all_demo_users();
        ViewBag.user = user;
        return View("ListUser");
    }

    // [HttpPost]
    public IActionResult store()
    {
        StringValues username_values;
        StringValues password_values;

        Request.Form.TryGetValue("username", out username_values);
        Request.Form.TryGetValue("password", out password_values);

        string username = !StringValues.IsNullOrEmpty(username_values)
            ? username_values.ToString()
            : string.Empty;
        string password = !StringValues.IsNullOrEmpty(password_values)
            ? password_values.ToString()
            : string.Empty;
        ViewBag.username = username;
        var user = DemoUserQuery.get_demo_user_by_username_password(username, password);
        ViewBag.oneuser = user.IsNullOrEmpty() ? "Empty" : user[0].ToString();
        return View("user");
    }

    // ========================================================================
}

/* EOF */
