using System.Diagnostics;
using Microsoft.Data.SqlClient;

// Trang tổng quan các khóa học
class BriefCoursePage
{
    public int total_num_pages;
    public List<BriefCourseCard> courses = new();

    public BriefCoursePage(
        int current_page = 1,
        int num_displayed_courses = 20,
        string? search_by_course_name = null,
        string? search_by_teacher_name = null,
        string? search_by_subject_name = null
    )
    {
        Database.exec(
            delegate(SqlConnection conn)
            {
                // Truy vấn tổng số khóa học
                Query q = new(Tbl.course);
                q.join(Field.semester__course_id, Field.course__id);
                q.WhereQuery(Field.semester__id, SemesterQuery.get_latest_semester_id_query("s"));
                q.Where(Field.semester__status, [SemesterStatus.waiting, SemesterStatus.started]);
                int num_total_courses = q.count(conn);
                // Suy ra tổng số trang
                this.total_num_pages = (int)
                    Math.Ceiling((double)num_total_courses / num_displayed_courses);
                // Danh sách khóa học
                this.courses = BriefCoursePage.get_page(
                    conn,
                    current_page,
                    num_displayed_courses,
                    search_by_course_name,
                    search_by_teacher_name,
                    search_by_subject_name
                );
            }
        );
    }

    public static List<BriefCourseCard> get_page(
        SqlConnection conn,
        int page = 1,
        int num_objs = 20,
        string? search_by_course_name = null,
        string? search_by_teacher_name = null,
        string? search_by_subject_name = null
    )
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        List<BriefCourseCard> cards = new();
        Query q = BriefCourseCard.getQueryCreator();
        q.Where(Field.semester__status, [SemesterStatus.waiting, SemesterStatus.started]);
        // if (search_by_course_name != null)
        // {
        //     q.where_string_contains(Field.course__name, search_by_course_name);
        // }
        // if (search_by_teacher_name != null)
        // {
        //     q.where_string_contains(Field.teacher__name, search_by_teacher_name);
        // }
        // if (search_by_subject_name != null)
        // {
        //     q.where_string_contains(Field.subject__name, search_by_subject_name);
        // }
        q.orderBy(Field.semester__id, desc: true);
        q.offset(page, num_objs);
        cards = q.select<BriefCourseCard>(conn);
        stopwatch.Stop();
        TimeSpan elapsed = stopwatch.Elapsed;
        Console.WriteLine($"get_brief_course_cards");
        Console.WriteLine($"Number of courses: {cards.Count}");
        Console.WriteLine($"Time taken: {elapsed.TotalMilliseconds} ms");
        return cards;
    }
}

/* EOF */
