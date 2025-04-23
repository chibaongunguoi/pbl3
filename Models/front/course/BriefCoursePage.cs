using System.Diagnostics;
using Microsoft.Data.SqlClient;

// Trang tổng quan các khóa học
class BriefCoursePage
{
    public int maxPageNum;

    const int num_displayed_courses = 20;
    public BriefCoursePage(
    )
    {
        QDatabase.Exec(
            delegate(SqlConnection conn)
            {
                // Truy vấn tổng số khóa học
                Query q = new(Tbl.course);
                q.Join(Field.semester__course_id, Field.course__id);
                q.WhereQuery(Field.semester__id, SemesterQuery.getLatestSemesterIdQuery("s"));
                q.Where(Field.semester__status, [SemesterStatus.waiting, SemesterStatus.started]);
                maxPageNum = (int)
                    Math.Ceiling((double)q.Count(conn) / num_displayed_courses);
            }
        );
    }
}

/* EOF */
