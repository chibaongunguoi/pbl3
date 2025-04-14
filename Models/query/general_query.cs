using Microsoft.Data.SqlClient;

static class GeneralQuery
{
    public static void update_course_states()
    {
        DateOnly today = DateOnly.FromDateTime(DateTime.Now);
        DateOnly yesterday = today.AddDays(-1);

        void start_courses(SqlConnection conn)
        {
            Query q = new(Table.semester);
            q.where_(Field.semester__start_date, today);
            q.where_(Field.semester__state, SemesterState.waiting);

            Query course_ids_query = q;
            course_ids_query.output(Field.semester__course_id);
            List<string> course_ids = Database.exec_query(
                conn,
                course_ids_query.get_select_query()
            );

            Query update_semester_query = q;
            update_semester_query.set_(Field.semester__state, SemesterState.started);
            update_semester_query.update(conn);

            Query update_course_query = new(Table.course);
            update_course_query.set_(Field.course__state, CourseState.started);
            update_course_query.where_str(Field.course__id, course_ids);
            update_course_query.update(conn);
        }

        void finish_courses(SqlConnection conn)
        {
            Query q = new(Table.semester);
            q.where_(Field.semester__finish_date, yesterday);
            q.where_(Field.semester__state, SemesterState.started);

            Query course_ids_query = q;
            course_ids_query.output(Field.semester__course_id);
            List<string> course_ids = Database.exec_query(
                conn,
                course_ids_query.get_select_query()
            );

            Query update_semester_query = q;
            update_semester_query.set_(Field.semester__state, SemesterState.finished);
            update_semester_query.update(conn);

            Query update_course_query = new(Table.course);
            update_course_query.set_(Field.course__state, CourseState.finished);
            update_course_query.where_str(Field.course__id, course_ids);
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
