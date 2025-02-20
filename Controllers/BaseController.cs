// WARN: Đây là code của hệ thống, hạn chế sửa đổi!
//
// NOTE: Mọi Controller đều phải kế thừa từ lớp BaseController này
// để có những chức năng cơ bản, bao gồm báo lỗi.
//
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using REPO.Models;

namespace REPO.Controllers;

public class BaseController : Controller
{
    // ========================================================================
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(
            new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }
        );
    }

    // ========================================================================
}

/* EOF */
