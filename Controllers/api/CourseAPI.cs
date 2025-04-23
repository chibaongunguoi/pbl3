using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace REPO.Controllers;

[Route("courseAPI")]
public class CourseAPI : BaseController
{
    [HttpGet("BriefCoursePage")]
    public IActionResult BriefCoursePage(int currentPage)
    {
        int numObjs = 20;
        List<BriefCourseCard> cards = new();
        Query q = BriefCourseCard.getQueryCreator();
        q.Where(Field.semester__status, [SemesterStatus.waiting, SemesterStatus.started]);
        q.OrderBy(Field.semester__id, desc: true);
        q.offset(currentPage, numObjs);
        QDatabase.exec(conn => cards = q.Select<BriefCourseCard>(conn));
        return PartialView(PartialList.BriefCourseCard, cards);
    }

    [HttpGet("TeacherProfile")]
    public IActionResult TeacherProfile(int currentPage)
    {
        int? tchId = UrlQuery.getInt(Request.Query, UrlKey.tchId);
        List<BriefCourseCard> cards = new();
        Query q = BriefCourseCard.getQueryCreator();
        q.Where(Field.teacher__id, tchId);
        q.OrderBy(Field.semester__id, desc: true);
        q.offset(currentPage, 20);
        QDatabase.exec(conn => cards = q.Select<BriefCourseCard>(conn));
        return PartialView(PartialList.BriefCourseCard, cards);
    }

    [HttpGet("StudentCourse")]
    public IActionResult StudentCourse(int stuId)
    {
        List<ManageCourseCard> cards = new();
        int tableIdx = 1;
        int pos = 0;
        Query q = ManageCourseCard.GetStudentCourseQueryCreator(stuId);
        QDatabase.exec(conn => q.Select(conn, reader => cards.Add(ManageCourseCard.getStudentCourseCard(reader, ref pos, ref tableIdx))));
        return PartialView(PartialList.ManageCourseCard, cards);
    }
}