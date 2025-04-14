using Microsoft.AspNetCore.Mvc;
using REPO.Models;

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
   public IActionResult GetPagination(int currentPage)
    {
        Console.WriteLine($"Received page: {currentPage}");
        return PartialView("_PaginationAjax", currentPage);
    }


    // public IActionResult GetData(int pageIndex)
    // {
    //     // Logic lấy dữ liệu theo pageIndex
    //     // return PartialView("_DataPartial", data);
    // }
}