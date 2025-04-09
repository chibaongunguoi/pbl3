namespace REPO.Controllers;

using Microsoft.AspNetCore.Mvc;

// using Microsoft.IdentityModel.Tokens;

public class AuthController : BaseController
{
    // ========================================================================

    public IActionResult Login()
    {
        // var user1001 = DemoUserQuery.get_demo_user_by_id(1002);
        // ViewBag.oneuser = user1001.IsNullOrEmpty() ? "Empty" : user1001[0].ToString();
        return View("Login");
    }

    public IActionResult SignUp()
    {
        return View("SignUp");
    }

    public IActionResult Listuser()
    {
        // var user = DemoUserQuery.get_all_demo_users();
        // ViewBag.user = user;
        return View("ListUser");
    }

    [HttpPost]
    public IActionResult store(LoginForm form)
    {
        string username = form.username;
        string password = form.password;

        Console.WriteLine($"Username: {username}");
        Console.WriteLine($"Password: {password}");

        List<User> query_result = new();
        query_result = Database.exec_list<User>(conn =>
            AccountQuery<User>.get_account_by_username_password(conn, username, password)
        );

        if (query_result.Count == 0)
        {
            return Redirect("Login");
        }

        User user = query_result[0];
        ViewBag.oneuser =
            $"Vai trò: {AccountQuery<User>.get_latest_table().ToString()}, Họ và tên: {user.name}.";

        SessionManager.log_in(HttpContext.Session, user.id);
        HttpContext.Session.SetString("userName", user.name);
        HttpContext.Session.SetInt32("userId", user.id);
        return Redirect("/");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Remove("userName"); // Xóa một session cụ thể
        HttpContext.Session.Remove("userId");

        SessionManager.log_out(HttpContext.Session);

        HttpContext.Session.Clear();
        return Redirect("http://localhost:5022/Auth/Login");
    }

    // ========================================================================
}

/* EOF */
