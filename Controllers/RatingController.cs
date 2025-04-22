namespace REPO.Controllers;

using Microsoft.AspNetCore.Mvc;

// using Microsoft.IdentityModel.Tokens;

public class RatingController : BaseController
{
    // ========================================================================

    public IActionResult Manage()
    {
        // var user1001 = DemoUserQuery.get_demo_user_by_id(1002);
        // ViewBag.oneuser = user1001.IsNullOrEmpty() ? "Empty" : user1001[0].ToString();
        return View();
    }

   
    // ========================================================================
}

/* EOF */
