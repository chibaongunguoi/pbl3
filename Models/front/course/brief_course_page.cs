using System.Diagnostics;
using Microsoft.Data.SqlClient;

// Trang tổng quan các khóa học
class BriefCoursePage
{
    public int maxPageNum;

    public BriefCoursePage(
        int num_displayed_courses = 20
    )
    {
        QDatabase.exec(
            delegate(SqlConnection conn)
            {
                // Truy vấn tổng số khóa học
                Query q = new(Tbl.course);
                q.join(Field.semester__course_id, Field.course__id);
                q.WhereQuery(Field.semester__id, SemesterQuery.get_latest_semester_id_query("s"));
                q.Where(Field.semester__status, [SemesterStatus.waiting, SemesterStatus.started]);
                int num_total_courses = q.count(conn);
                // Suy ra tổng số trang
                this.maxPageNum = (int)
                    Math.Ceiling((double)num_total_courses / num_displayed_courses);
                // Danh sách khóa học
            }
        );
    }
}

/* EOF */
