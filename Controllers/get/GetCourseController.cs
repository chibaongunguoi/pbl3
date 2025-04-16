using Microsoft.AspNetCore.Mvc;

namespace REPO.Controllers;

[Route("GetCourse")]
public class GetCourseController : Controller
{
    [HttpGet("getRatingCards")]
    public IActionResult getRatingCards(int currentPage)
    {
        int course_id = Session.get_int(Request.Query, "course_id") ?? 0;
        List<RatingCard> rating_cards = new();
        Database.exec(conn =>
            rating_cards = DetailedCoursePage.get_page(conn, course_id, currentPage)
        );
        return PartialView("_RatingCardList", rating_cards);
    }

    [HttpGet("getBriefCourseCards")]
    public IActionResult getBriefCourseCards(int currentPage)
    {
        List<BriefCourseCard> cards = new();
        Database.exec(conn => cards = BriefCoursePage.get_page(conn, currentPage));
        return PartialView("_SemesterCardList", cards);
    }
}
