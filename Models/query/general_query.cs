using Microsoft.Data.SqlClient;

static class GeneralQuery
{
    public static void update_course_status()
    {
        DateOnly today = DateOnly.FromDateTime(DateTime.Now);
        DateOnly yesterday = today.AddDays(-1);

        void start_courses(SqlConnection conn)
        {
            Query q = new(Tbl.semester);
            q.Where(Field.semester__start_date, today);
            q.Where(Field.semester__status, SemesterStatus.waiting);

            Query course_ids_query = q;
            course_ids_query.output(Field.semester__course_id);
            List<int> course_ids = new();
            course_ids_query.select(conn, reader => course_ids.Add(DataReader.getInt(reader)));

            Query update_semester_query = q;
            update_semester_query.Set(Field.semester__status, SemesterStatus.started);
            update_semester_query.update(conn);

            Query update_course_query = new(Tbl.course);
            update_course_query.Set(Field.course__status, CourseStatus.started);
            update_course_query.Where(Field.course__id, course_ids);
            update_course_query.update(conn);
        }

        void finish_courses(SqlConnection conn)
        {
            Query q = new(Tbl.semester);
            q.Where(Field.semester__finish_date, yesterday);
            q.Where(Field.semester__status, SemesterStatus.started);

            Query course_ids_query = q;
            course_ids_query.output(Field.semester__course_id);
            List<string> course_ids = new();
            course_ids_query.select(conn, reader => course_ids.Add(DataReader.getStr(reader)));

            Query update_semester_query = q;
            update_semester_query.Set(Field.semester__status, SemesterStatus.finished);
            update_semester_query.update(conn);

            Query update_course_query = new(Tbl.course);
            update_course_query.Set(Field.course__status, CourseStatus.finished);
            update_course_query.Where(Field.course__id, course_ids);
            update_course_query.update(conn);
        }

        Database.exec(
            delegate(SqlConnection conn)
            {
                start_courses(conn);
                finish_courses(conn);
            }
        );
    }
}
