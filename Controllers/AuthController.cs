using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace REPO.Controllers;

using Microsoft.AspNetCore.Mvc;

public class AuthController : BaseController
{

    public IActionResult AccessDenied()
    {
        return View();
    }   

    public IActionResult Login()
    {
        if (User.Identity?.IsAuthenticated ?? false)
        {
            return Redirect("/");
        }
        return View("Login");
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginForm form)
    {
        if (!ModelState.IsValid)
            return View(form);

        Account? account = null;
        string table = "";
        string role = "";
        QDatabase.Exec(conn => AuthQuery.Login(conn, form, ref account, ref table, ref role));
        if (account is null)
        {
            ModelState.AddModelError(string.Empty, "Đăng nhập không thành công");
            return View(form);
        }

        var claims = new List<Claim>
        {
            new (ClaimTypes.Name, account.Username),
            new (ClaimTypes.Role, role),
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


    public IActionResult SignUp()
    {
        if (User.Identity?.IsAuthenticated ?? false)
        {
            return Redirect("/");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignUp(StudentSignUpForm form)
    {
        if (!ModelState.IsValid)
            return View(form);

        Account? account = null;
        QDatabase.Exec(conn => AuthQuery.SignUp(conn, ModelState, form, ref account));

        if (account is null)
        {
            return View(form);
        }

        var claims = new List<Claim>
        {
            new (ClaimTypes.Name, account?.Username ?? string.Empty),
            new (ClaimTypes.Role, UserRole.Student),
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

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Redirect("/Auth/Login");
    }
}

/* EOF */
