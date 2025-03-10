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

        string username = !StringValues.IsNullOrEmpty(username_values)
            ? username_values.ToString()
            : string.Empty;
        string password = !StringValues.IsNullOrEmpty(password_values)
            ? password_values.ToString()
            : string.Empty;

        // var stu_query_result = StudentQuery.get_student_by_username_password(username, password);
        //
        // if (!stu_query_result.IsNullOrEmpty())
        // {
        //     ViewBag.oneuser = stu_query_result[0].ToString();
        //     return View("user");
        // }
        //
        // var tch_query_result = TeacherQuery.get_teacher_by_username_password(username, password);
        //
        // if (!tch_query_result.IsNullOrEmpty())
        // {
        //     ViewBag.oneuser = tch_query_result[0].ToString();
        //     return View("user");
        // }

        var query_result = Database.exec_list<User>(conn =>
            AccountQuery<User>.get_account_by_username_password(conn, username, password)
        );

        if (query_result.Count == 0)
        {
            return View("Login");
        }

        User user = query_result[0];
        ViewBag.oneuser =
            $"Vai trò: {AccountQuery<User>.get_latest_table().ToString()}, Họ và tên: {user.fullname}.";
        return View("user");
    }

    // ========================================================================
}

/* EOF */
