using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace REPO.Controllers;

using Microsoft.AspNetCore.Mvc;

public class AuthController : BaseController
{
    public IActionResult Login()
    {
        if (User.Identity?.IsAuthenticated ?? false)
        {
            return Redirect("/");
        }
        return View("Login");
    }

    public IActionResult SignUp()
    {
        if (User.Identity?.IsAuthenticated ?? false)
        {
            return Redirect("/");
        }

        if (SessionForm.displaying_error)
        {
            SessionForm.displaying_error = false;
        }
        else
        {
            SessionForm.errors.Clear();
        }
        return View("SignUp");
    }

    [HttpPost]
    public async Task<IActionResult> submit_sign_up(StudentSignUpForm form)
    {
        StudentSignUpForm.Log log = form.execute();
        if (!log.success)
        {
            SessionForm.displaying_error = true;
            SessionForm.errors = log.errors;
            return Redirect("SignUp");
        }

        int studentId = log.stu_id ?? 0;
        string name = "";

        // Get student name
        QDatabase.Exec(conn =>
        {
            List<User> users = CommonQuery<User>.get_record_by_id(conn, Tbl.student, studentId);
            name = users[0].Name;
        });

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, studentId.ToString()),
            new Claim(ClaimTypes.Role, UserRole.Student),
            new Claim(ClaimTypes.Name, name),
        };

        var claimsIdentity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme
        );
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7),
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties
        );
        return Redirect("/");
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Listuser()
    {
        return View("ListUser");
    }

    [HttpPost]
    public async Task<IActionResult> store(LoginForm form)
    {
        Console.WriteLine("Yeah");
        List<Account> query_result = new();
        string table = "";
        QDatabase.Exec(conn =>
            AccountQuery<Account>.getAccountByUsernamePassword(
                conn,
                form.Username,
                form.Password,
                ref query_result,
                ref table
            )
        );

        if (query_result.Count == 0)
        {
            return Redirect("Login");
        }

        int id = query_result[0].Id;
        string role = "";
        string name = "";

        // Get user role and name
        switch (table)
        {
            case Tbl.student:
                role = UserRole.Student;
                break;
            case Tbl.teacher:
                role = UserRole.Teacher;
                break;
            case Tbl.admin:
                role = UserRole.Admin;
                break;
        }

        // Get name for students and teachers
        if (table is Tbl.student or Tbl.teacher)
        {
            QDatabase.Exec(conn =>
            {
                List<User> users = CommonQuery<User>.get_record_by_id(conn, table, id);
                name = users[0].Name;
            });
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, id.ToString()),
            new Claim(ClaimTypes.Role, role),
            new Claim(ClaimTypes.Name, name),
        };

        var claimsIdentity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme
        );
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7),
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties
        );

        return Redirect("/");
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Redirect("/Auth/Login");
    }
}

/* EOF */
