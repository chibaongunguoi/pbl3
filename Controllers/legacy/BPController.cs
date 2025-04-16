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
        int currentPage,
        int max_index_page,
        string context_url,
        string context_component
    )
    {
        return PartialView(
            "_PaginationAjax",
            ValueTuple.Create(currentPage, max_index_page, context_url, context_component)
        );
    }

    // public IActionResult GetData(int pageIndex)
    // {
    //     // Logic lấy dữ liệu theo pageIndex
    //     // return PartialView("_DataPartial", data);
    // }
}

