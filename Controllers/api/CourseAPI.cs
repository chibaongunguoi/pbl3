using Microsoft.AspNetCore.Mvc;

namespace REPO.Controllers;

[Route("courseAPI")]
public class CourseAPI : BaseController
{
    [HttpGet("BriefCoursePage")]
    public IActionResult BriefCoursePage(int currentPage, BriefCourseFilterForm filterForm)
    {
        int numObjs = 20;
        List<BriefCourseCard> cards = [];
        Query q = BriefCourseCard.GetQueryCreator();
        q.Where(Field.semester__status, [SemesterStatus.waiting, SemesterStatus.started]);
        q.OrderBy(Field.semester__id, desc: true);
        q.Offset(currentPage, numObjs);
        if (filterForm.SubjectName is not null)
        {
            q.WhereNString(Field.subject__name, filterForm.SubjectName);
        }
        if (filterForm.Grade != 0)
        {
            q.Where(Field.subject__grade, filterForm.Grade);
        }
        if (filterForm.CourseName is not null)
        {
            q.WhereContains(Field.course__name, filterForm.CourseName);
        }

        if (filterForm.Gender is not null)
        {
            q.Where(Field.teacher__gender, filterForm.Gender);
        }
        QDatabase.Exec(conn => cards = q.Select<BriefCourseCard>(conn));
        return PartialView(PartialList.BriefCourseCard, cards);
    }

    [HttpGet("TeacherProfile")]
    public IActionResult TeacherProfile(int currentPage)
    {
        int numObjs = 20;
        int? tchId = UrlQuery.getInt(Request.Query, UrlKey.tchId);
        List<BriefCourseCard> cards = new();
        Query q = BriefCourseCard.GetQueryCreator();
        q.Where(Field.teacher__id, tchId);
        q.OrderBy(Field.semester__id, desc: true);
        q.Offset(currentPage, numObjs);
        QDatabase.Exec(conn => cards = q.Select<BriefCourseCard>(conn));
        return PartialView(PartialList.BriefCourseCard, cards);
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

