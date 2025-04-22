using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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
        int? teacherId = UrlQuery.getInt(Request.Query, UrlKey.tchId);
        int numObjs = 20;
        List<BriefCourseCard> cards = new();
        Query q = BriefCourseCard.getQueryCreator();
        q.Where(Field.teacher__id, teacherId);
        q.orderBy(Field.semester__id, desc: true);
        q.offset(currentPage, numObjs);
        QDatabase.exec(conn => cards = q.select<BriefCourseCard>(conn));
        return PartialView(PartialList.BriefCourseCard, cards);
    }
}
