using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace REPO.Controllers;

[Route("courseAPI")]
public class CourseAPI : BaseController
{
    private static Query BriefCoursePageQuery(BriefCourseFilter filter, string? role = null, string? username = null)
    {
        Query q = BriefCourseCard.GetQueryCreator(role, username);
        q.Where(Field.semester__status, [SemesterStatus.waiting, SemesterStatus.started]);
        q.OrderBy(Field.semester__id, desc: true);
        if (filter.SubjectName is not null)
        {
            q.WhereNString(Field.subject__name, filter.SubjectName);
        }
        if (filter.Grade is not null)
        {
            q.Where(Field.subject__grade, filter.Grade);
        }
        if (filter.CourseName is not null)
        {
            q.WhereContains(Field.course__name, filter.CourseName);
        }

        if (filter.Gender is not null)
        {
            q.Where(Field.teacher__gender, filter.Gender);
        }
        return q;
    }

    [HttpGet("BriefCoursePage")]
    public IActionResult BriefCoursePage(PaginationInfo paginationInfo, BriefCourseFilter filter)
    {
        string? role = User.FindFirst(ClaimTypes.Role)?.Value;
        string? username = User.FindFirst(ClaimTypes.Name)?.Value;
        List<BriefCourseCard> cards = [];
        Query q = BriefCoursePageQuery(filter, role, username);
        q.Offset(paginationInfo.CurrentPage, paginationInfo.ItemsPerPage);
        int pos = 0;
        QDatabase.Exec(conn => q.Select(conn, reader => cards.Add(BriefCourseCard.GetCard(reader, ref pos, role))));
        return PartialView(PartialList.BriefCourseCard, cards);

    }

    [HttpGet("BriefCoursePage/Pagination")]
    public IActionResult BriefCoursePagePagination(PaginationInfo paginationInfo, BriefCourseFilter filter, string contextUrl, string contextComponent)
    {
        QDatabase.Exec(conn => paginationInfo.TotalItems = BriefCoursePageQuery(filter).Count(conn));
        return PartialView(
            "_PaginationAjax",
            ValueTuple.Create(paginationInfo, contextUrl, contextComponent)
        );
    }

    [HttpGet("TeacherProfile")]
    public IActionResult TeacherProfile(PaginationInfo paginationInfo, BriefCourseFilter filter, int tchId)
    {
        Console.WriteLine($"TeacherProfile: {tchId}");
        string? role = User.FindFirst(ClaimTypes.Role)?.Value;
        string? username = User.FindFirst(ClaimTypes.Name)?.Value;
        Query q = BriefCoursePageQuery(filter, role, username);
        q.Where(Field.teacher__id, tchId);
        q.Offset(paginationInfo.CurrentPage, paginationInfo.ItemsPerPage);
        List<BriefCourseCard> cards = [];
        int pos = 0;
        QDatabase.Exec(conn => q.Select(conn, reader => cards.Add(BriefCourseCard.GetCard(reader, ref pos, role))));
        return PartialView(PartialList.BriefCourseCard, cards);
    }

    [HttpGet("TeacherProfile/Pagination")]
    public IActionResult TeacherProfilePagination(PaginationInfo paginationInfo, BriefCourseFilter filter, int tchId, string contextUrl, string contextComponent)
    {
        Query q = BriefCoursePageQuery(filter);
        q.Where(Field.teacher__id, tchId);
        QDatabase.Exec(conn => paginationInfo.TotalItems = q.Count(conn));
        return PartialView(
            "_PaginationAjax",
            ValueTuple.Create(paginationInfo, contextUrl, contextComponent)
        );
    }

    [HttpGet("StudentCourse")]
    public IActionResult StudentCourse(string username)
    {
        List<ManageCourseCard> cards = [];
        int tableIdx = 1;
        int pos = 0;
        Query q = ManageCourseCard.GetStudentCourseQueryCreator(username);
        QDatabase.Exec(conn =>
            q.Select(
                conn,
                reader =>
                    cards.Add(ManageCourseCard.getStudentCourseCard(reader, ref pos, ref tableIdx))
            )
        );
        return PartialView(PartialList.ManageCourseCard, cards);
    }
}

