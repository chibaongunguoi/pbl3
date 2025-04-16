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
            q.Where(Tbl.semester, Fld.start_date, today);
            q.Where(Tbl.semester, Fld.state, SemesterState.waiting);

            Query course_ids_query = q;
            course_ids_query.output(Tbl.semester, Fld.course_id);
            List<string> course_ids = Database.exec_query(conn, course_ids_query.selectQuery());

            Query update_semester_query = q;
            update_semester_query.Set(Tbl.semester, Fld.state, SemesterState.started);
            update_semester_query.update(conn);

            Query update_course_query = new(Tbl.course);
            update_course_query.Set(Tbl.course, Fld.state, CourseState.started);
            update_course_query.Where(Tbl.course, Fld.id, course_ids);
            update_course_query.update(conn);
        }

        void finish_courses(SqlConnection conn)
        {
            Query q = new(Tbl.semester);
            q.Where(Tbl.semester, Fld.finish_date, yesterday);
            q.Where(Tbl.semester, Fld.state, SemesterState.started);

            Query course_ids_query = q;
            course_ids_query.output(Tbl.semester, Fld.course_id);
            List<string> course_ids = Database.exec_query(conn, course_ids_query.selectQuery());

            Query update_semester_query = q;
            update_semester_query.Set(Tbl.semester, Fld.state, SemesterState.finished);
            update_semester_query.update(conn);

            Query update_course_query = new(Tbl.course);
            update_course_query.Set(Tbl.course, Fld.state, CourseState.finished);
            update_course_query.Where(Tbl.course, Fld.id, course_ids);
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
