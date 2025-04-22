using Microsoft.AspNetCore.Mvc;

namespace REPO.Controllers;

[Route("GetCourse")]
public class GetCourseController : Controller
{
    [HttpGet("getRatingCards")]
    public IActionResult getRatingCards(int currentPage)
    {
        int? course_id = Session.getInt(Request.Query, UrlKey.courseId);
        if (course_id is null)
        {
            return PartialView("_RatingCardList", new List<RatingCard>());
        }
        List<RatingCard> rating_cards = new();
        QDatabase.exec(conn =>
            rating_cards = DetailedCoursePage.get_page(conn, course_id.Value, currentPage)
        );
        return PartialView("_RatingCardList", rating_cards);
    }

    [HttpGet("getBriefCourseCards")]
    public IActionResult getBriefCourseCards(int currentPage)
    {
        List<BriefCourseCard> cards = new();
        QDatabase.exec(conn => cards = BriefCoursePage.get_page(conn, currentPage));
        return PartialView(PartialViewKey.semesterCardList, cards);
    }

    [HttpGet("getBriefCourseCardsByTchId")]
    public IActionResult getBriefCourseCardsByTchId(int currentPage)
    {
        int? tchId = Session.getInt(Request.Query, UrlKey.tchId);
        List<BriefCourseCard> cards = new();
        if (tchId is null)
        {
            return PartialView(PartialViewKey.semesterCardList, cards);
        }
        Query q = BriefCourseCard.getQueryCreator();
        q.Where(Field.teacher__id, tchId);
        q.orderBy(Field.semester__id, desc: true);
        q.offset(currentPage, 20);
        QDatabase.exec(conn => cards = q.select<BriefCourseCard>(conn));
        return PartialView(PartialViewKey.semesterCardList, cards);
    }
}
