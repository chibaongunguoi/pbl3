using Microsoft.AspNetCore.Mvc;

namespace REPO.Controllers;

[Route("BPController")]
public class BPController : Controller
{
    public IActionResult Index()
    {
        int defaultPageIndex = 1;
        return View(defaultPageIndex);
    }

    [HttpGet("GetPagination")]
    public IActionResult GetPagination(
        PaginationInfo paginationInfo,
        string contextUrl,
        string contextComponent
    )
    {
        return PartialView(
            "_PaginationAjax",
            ValueTuple.Create(paginationInfo, contextUrl, contextComponent)
        );
    }

    // public IActionResult GetData(int pageIndex)
    // {
    //     // Logic lấy dữ liệu theo pageIndex
    //     // return PartialView("_DataPartial", data);
    // }
}

