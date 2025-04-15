using Microsoft.Data.SqlClient;

static class GeneralQuery
{
    public static void update_course_states()
    {
        DateOnly today = DateOnly.FromDateTime(DateTime.Now);
        DateOnly yesterday = today.AddDays(-1);

        void start_courses(SqlConnection conn)
        {
            QueryCreator q = new(Tbl.semester);
            q.Where(Tbl.semester, Fld.start_date, today);
            q.WhereStr(Tbl.semester, Fld.state, SemesterState.waiting);

            QueryCreator course_ids_query = q;
            course_ids_query.output(QPiece.dot(Tbl.semester, Fld.course_id));
            List<string> course_ids = Database.exec_query(
                conn,
                course_ids_query.get_select_query()
            );

            QueryCreator update_semester_query = q;
            update_semester_query.SetStr(Tbl.semester, Fld.state, SemesterState.started);
            update_semester_query.update(conn);

            QueryCreator update_course_query = new(Tbl.course);
            update_course_query.SetStr(Tbl.course, Fld.state, CourseState.started);
            update_course_query.WhereStrList(Tbl.course, Fld.id, course_ids);
            update_course_query.update(conn);
        }

        void finish_courses(SqlConnection conn)
        {
            QueryCreator q = new(Tbl.semester);
            q.Where(Tbl.semester, Fld.finish_date, yesterday);
            q.WhereStr(Tbl.semester, Fld.state, SemesterState.started);

            QueryCreator course_ids_query = q;
            course_ids_query.output(QPiece.dot(Tbl.semester, Fld.course_id));
            List<string> course_ids = Database.exec_query(
                conn,
                course_ids_query.get_select_query()
            );

            QueryCreator update_semester_query = q;
            update_semester_query.SetStr(Tbl.semester, Fld.state, SemesterState.finished);
            update_semester_query.update(conn);

            QueryCreator update_course_query = new(Tbl.course);
            update_course_query.SetStr(Tbl.course, Fld.state, CourseState.finished);
            update_course_query.WhereStrList(Tbl.course, Fld.id, course_ids);
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
