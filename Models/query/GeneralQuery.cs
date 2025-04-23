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
            // q.Where(Field.semester__start_date, today);
            q.WhereClause($"{Field.semester__start_date} <= {QPiece.toStr(today)}");
            q.Where(Field.semester__status, SemesterStatus.waiting);

            Query course_ids_query = q;
            course_ids_query.output(Field.semester__course_id);
            List<int> course_ids = new();
            course_ids_query.Select(conn, reader => course_ids.Add(DataReader.getInt(reader)));

            Query update_semester_query = q;
            update_semester_query.Set(Field.semester__status, SemesterStatus.started);
            update_semester_query.Update(conn);

            Query update_course_query = new(Tbl.course);
            update_course_query.Set(Field.course__status, CourseStatus.started);
            update_course_query.Where(Field.course__id, course_ids);
            update_course_query.Update(conn);
        }

        void finish_courses(SqlConnection conn)
        {
            Query q = new(Tbl.semester);
            // q.Where(Field.semester__finish_date, yesterday);
            // q.Where(Field.semester__status, SemesterStatus.started);
            q.WhereClause($"{Field.semester__finish_date} <= {QPiece.toStr(yesterday)}");
            q.Where(Field.semester__status, [SemesterStatus.waiting, SemesterStatus.started]);

            Query course_ids_query = q;
            course_ids_query.output(Field.semester__course_id);
            List<int> course_ids = new();
            course_ids_query.Select(conn, reader => course_ids.Add(DataReader.getInt(reader)));

            Query update_semester_query = q;
            update_semester_query.Set(Field.semester__status, SemesterStatus.finished);
            update_semester_query.Update(conn);

            Query update_course_query = new(Tbl.course);
            update_course_query.Set(Field.course__status, CourseStatus.finished);
            update_course_query.Where(Field.course__id, course_ids);
            update_course_query.Update(conn);
        }

        QDatabase.exec(
            delegate(SqlConnection conn)
            {
                start_courses(conn);
                finish_courses(conn);
            }
        );
    }
}
