using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace REPO.Controllers;

[Authorize(Roles = UserRole.Student)]
public class StudentController : BaseController
{
    public IActionResult Course()
    {
        return View();
    }
}
