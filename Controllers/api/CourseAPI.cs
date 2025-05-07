using Microsoft.AspNetCore.Mvc;

namespace REPO.Controllers;

[Route("courseAPI")]
public class CourseAPI : BaseController
{
    private static Query BriefCoursePageQuery(BriefCourseFilter filter)
    {
        Query q = BriefCourseCard.GetQueryCreator();
        q.Where(Field.semester__status, [SemesterStatus.waiting, SemesterStatus.started]);
        q.OrderBy(Field.semester__id, desc: true);
        if (filter.SubjectName is not null)
        {
            q.WhereNString(Field.subject__name, filter.SubjectName);
        }
        if (filter.Grade != 0)
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
        List<BriefCourseCard> cards = [];
        Query q = BriefCoursePageQuery(filter);
        q.Offset(paginationInfo.CurrentPage, paginationInfo.ItemsPerPage);
        QDatabase.Exec(conn => cards = q.Select<BriefCourseCard>(conn));
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
        Query q = BriefCoursePageQuery(filter);
        q.Where(Field.teacher__id, tchId);
        q.Offset(paginationInfo.CurrentPage, paginationInfo.ItemsPerPage);
        List<BriefCourseCard> cards = [];
        QDatabase.Exec(conn => cards = q.Select<BriefCourseCard>(conn));
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

