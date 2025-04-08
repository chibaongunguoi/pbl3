namespace REPO.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

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

    // [HttpPost]
    public IActionResult store()
    {
        StringValues username_values;
        StringValues password_values;

        Request.Form.TryGetValue("username", out username_values);
        Request.Form.TryGetValue("password", out password_values);

        string id_ = !StringValues.IsNullOrEmpty(username_values)
            ? username_values.ToString()
            : string.Empty;
        string password = !StringValues.IsNullOrEmpty(password_values)
            ? password_values.ToString()
            : string.Empty;

        int id = 0;
        List<User> query_result = new();
        try
        {
            id = int.Parse(id_);
            query_result = Database.exec_list<User>(conn =>
                AccountQuery<User>.get_account_by_id_password(conn, id, password)
            );

            if (query_result.Count == 0)
            {
                throw new();
            }
        }
        catch
        {
            return View("Login");
        }

        User user = query_result[0];
        ViewBag.oneuser =
            $"Vai trò: {AccountQuery<User>.get_latest_table().ToString()}, Họ và tên: {user.name}.";

        HttpContext.Session.SetString("userName", user.name);
        HttpContext.Session.SetInt32("userId", user.id);
        return Redirect("/");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Remove("userName"); // Xóa một session cụ thể
        HttpContext.Session.Remove("userId");
        HttpContext.Session.Clear();
        return Redirect("http://localhost:5022/Auth/Login");
    }

    // ========================================================================
}

/* EOF */
