using Microsoft.AspNetCore.Mvc;

namespace REPO.Controllers;

[Route("ratingAPI")]
public class RatingAPI : BaseController
{
    private static Query DetailedCoursePageQuery(int courseId, int? stars)
    {
        Query q = RatingCard.get_query_creator();
        q.Where(Field.semester__course_id, courseId);
        q.OrderBy(Field.rating__timestamp, desc: true);

        if (stars is not null)
        {
            q.Where(Field.rating__stars, stars);
        }
        return q;
    }

    [HttpGet("DetailedCoursePage")]
    public IActionResult DetailedCoursePage_(PaginationInfo paginationInfo, int courseId, int? stars)
    {
        Query q = DetailedCoursePageQuery(courseId, stars);
        q.Offset(paginationInfo.CurrentPage, paginationInfo.ItemsPerPage);
        List<RatingCard> cards = [];
        QDatabase.Exec(conn =>  q.Select(conn, reader => cards.Add(RatingCard.get_card(conn, reader))));
        return PartialView(PartialList.RatingCard, cards);
    }

    [HttpGet("DetailedCoursePage/Pagination")]
    public IActionResult DetailedCoursePagePagintation_(PaginationInfo paginationInfo, int courseId, int? stars, string contextUrl, string contextComponent)
    {
        Query q = DetailedCoursePageQuery(courseId, stars);
        QDatabase.Exec(conn => paginationInfo.TotalItems = q.Count(conn));
        return PartialView(
            "_PaginationAjax",
            ValueTuple.Create(paginationInfo, contextUrl, contextComponent)
        );
    }
}
