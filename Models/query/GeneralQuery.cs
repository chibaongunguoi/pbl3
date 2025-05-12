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
            q.WhereClause($"{Field.semester__start_date} <= {QPiece.toStr(today)}");
            q.Where(Field.semester__status, SemesterStatus.waiting);

            Query update_semester_query = q;
            update_semester_query.Set(Field.semester__status, SemesterStatus.started);
            update_semester_query.Update(conn);
        }

        void finish_courses(SqlConnection conn)
        {
            Query q = new(Tbl.semester);
            q.WhereClause($"{Field.semester__finish_date} < {QPiece.toStr(yesterday)}");
            q.Where(Field.semester__status, [SemesterStatus.waiting, SemesterStatus.started]);

            Query update_semester_query = q;
            update_semester_query.Set(Field.semester__status, SemesterStatus.finished);
            update_semester_query.Update(conn);

            q.Output(Field.semester__id);
            Query q2 = new(Tbl.request);
            q2.WhereInQuery(Field.request__semester_id, q.SelectQuery());
            q2.Where(Field.request__status, RequestStatus.waiting);
            q2.Delete(conn);
        }

        QDatabase.Exec(
            delegate(SqlConnection conn)
            {
                start_courses(conn);
                finish_courses(conn);
            }
        );
    }
}
