using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace REPO.Controllers;

[Route("ratingAPI")]
public class RatingAPI : BaseController
{
    [HttpGet("DetailedCoursePage")]
    [Authorize]
    public IActionResult detailedCoursePage(int currentPage)
    {
        int? courseId = UrlQuery.getInt(Request.Query, UrlKey.courseId);
        if (courseId is null)
        {
            return PartialView(PartialList.RatingCard, new List<RatingCard>());
        }
        
        // Verify course exists
        bool courseExists = false;
        QDatabase.Exec(conn =>
        {
            Query q = new(Tbl.course);
            q.Where(Field.course__id, courseId);
            courseExists = q.Count(conn) > 0;
        });
        
        if (!courseExists)
        {
            return PartialView(PartialList.RatingCard, new List<RatingCard>());
        }

        List<RatingCard> ratingCards = new();
        QDatabase.Exec(conn =>
            ratingCards = DetailedCoursePage.get_page(conn, courseId.Value, currentPage)
        );
        return PartialView(PartialList.RatingCard, ratingCards);
    }
}
