using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using REPO.Models;

namespace REPO.Controllers;

// using Microsoft.IdentityModel.Tokens;

public class UserController : BaseController
{
    // ========================================================================
     private readonly ILogger<UserController> _logger;

    public UserController(ILogger<UserController> logger)
    {
        _logger = logger;
    }
    public IActionResult Profile()
    {
        
        return View();
    }
    public IActionResult ChangePassword()
    {
        
        return View();
    }
   // ========================================================================
}

/* EOF */
