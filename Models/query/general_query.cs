using Microsoft.Data.SqlClient;

static class GeneralQuery
{
    public static void update_course_states()
    {
        DateOnly today = DateOnly.FromDateTime(DateTime.Now);
        DateOnly yesterday = today.AddDays(-1);

        void start_courses(SqlConnection conn)
        {
            Query q = new(Tbl.semester);
            q.Where(Field.semester__start_date, today);
            q.Where(Field.semester__state, SemesterState.waiting);

            Query course_ids_query = q;
            course_ids_query.output(Field.semester__course_id);
            List<int> course_ids = new();
            course_ids_query.select(conn, reader => course_ids.Add(DataReader.getInt(reader)));

            Query update_semester_query = q;
            update_semester_query.Set(Field.semester__state, SemesterState.started);
            update_semester_query.update(conn);

            Query update_course_query = new(Tbl.course);
            update_course_query.Set(Field.course__state, CourseState.started);
            update_course_query.Where(Field.course__id, course_ids);
            update_course_query.update(conn);
        }

        void finish_courses(SqlConnection conn)
        {
            Query q = new(Tbl.semester);
            q.Where(Field.semester__finish_date, yesterday);
            q.Where(Field.semester__state, SemesterState.started);

            Query course_ids_query = q;
            course_ids_query.output(Field.semester__course_id);
            List<string> course_ids = new();
            course_ids_query.select(conn, reader => course_ids.Add(DataReader.getStr(reader)));

            Query update_semester_query = q;
            update_semester_query.Set(Field.semester__state, SemesterState.finished);
            update_semester_query.update(conn);

            Query update_course_query = new(Tbl.course);
            update_course_query.Set(Field.course__state, CourseState.finished);
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
