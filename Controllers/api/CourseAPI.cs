using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace REPO.Controllers;

[Route("courseAPI")]
public class CourseAPI : BaseController
{
    [HttpGet("BriefCoursePage")]
    public IActionResult briefCoursePage(int currentPage)
    {
        int numObjs = 20;
        List<BriefCourseCard> cards = new();
        Query q = BriefCourseCard.getQueryCreator();
        q.Where(Field.semester__status, [SemesterStatus.waiting, SemesterStatus.started]);
        q.orderBy(Field.semester__id, desc: true);
        q.offset(currentPage, numObjs);
        QDatabase.exec(conn => cards = q.select<BriefCourseCard>(conn));
        return PartialView(PartialList.BriefCourseCard, cards);
    }

    [HttpGet("TeacherProfile")]
    public IActionResult teacherProfile(int currentPage)
    {
        int? tchId = UrlQuery.getInt(Request.Query, UrlKey.tchId);
        List<BriefCourseCard> cards = new();
        Query q = BriefCourseCard.getQueryCreator();
        q.Where(Field.teacher__id, tchId);
        q.orderBy(Field.semester__id, desc: true);
        q.offset(currentPage, 20);
        QDatabase.exec(conn => cards = q.select<BriefCourseCard>(conn));
        return PartialView(PartialList.BriefCourseCard, cards);
    }
}
