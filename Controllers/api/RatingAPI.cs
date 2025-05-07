using Microsoft.AspNetCore.Mvc;

namespace REPO.Controllers;

[Route("ratingAPI")]
public class RatingAPI : BaseController
{
    private static Query DetailedCoursePageQuery(int courseId)
    {
        Query q = RatingCard.get_query_creator();
        q.Where(Field.semester__course_id, courseId);
        q.OrderBy(Field.rating__timestamp, desc: true);
        return q;
    }

    [HttpGet("DetailedCoursePage")]
    public IActionResult detailedCoursePage(PaginationInfo paginationInfo, int courseId)
    {
        Query q = DetailedCoursePageQuery(courseId);
        q.Offset(paginationInfo.CurrentPage, paginationInfo.ItemsPerPage);
        List<RatingCard> cards = [];
        QDatabase.Exec(conn =>  q.Select(conn, reader => cards.Add(RatingCard.get_card(conn, reader))));
        return PartialView(PartialList.RatingCard, cards);
    }

    [HttpGet("DetailedCoursePage/Pagination")]
    public IActionResult detailedCoursePagePagintation(PaginationInfo paginationInfo, int courseId, string contextUrl, string contextComponent)
    {
        Query q = DetailedCoursePageQuery(courseId);
        QDatabase.Exec(conn => paginationInfo.TotalItems = q.Count(conn));
        return PartialView(
            "_PaginationAjax",
            ValueTuple.Create(paginationInfo, contextUrl, contextComponent)
        );
    }
}
