using Microsoft.Data.SqlClient;

static class CourseQuery
{
    public static void get_avg_rating(
        SqlConnection conn,
        int course_id,
        out double avg_rating,
        out int num_ratings
    )
    {
        Query q = new();
        // rating avg
        Query q2 = new(Tbl.rating);
        q2.Join(Field.semester__id, Field.rating__semester_id);
        q2.Where(Field.semester__course_id, course_id);
        q2.OutputAvgCastFloat(Field.rating__stars);
        q.OutputQuery(q2.SelectQuery());

        // rating count
        q2 = new(Tbl.rating);
        q2.Join(Field.semester__id, Field.rating__semester_id);
        q2.Where(Field.semester__course_id, course_id);
        q2.Output(QPiece.countAll);
        q.OutputQuery(q2.SelectQuery());

        double temp_avg_rating = 0;
        int temp_num_ratings = 0;

        q.Select(
            conn,
            delegate(SqlDataReader reader)
            {
                int pos = 0;
                temp_avg_rating = DataReader.getDouble(reader, pos++);
                temp_num_ratings = DataReader.getInt(reader, pos++);
            }
        );
        avg_rating = temp_avg_rating;
        num_ratings = temp_num_ratings;
    }

    public static bool checkStudentEnrolled(
        SqlConnection conn,
        int courseId,
        int stuId
    )
    {
        Query q = new(Tbl.request);
        q.Join(Field.semester__id, Field.request__semester_id);
        q.Join(Field.course__id, Field.semester__course_id);
        q.WhereQuery(Field.request__semester_id, SemesterQuery.getLatestSemesterIdQuery("s"));
        q.Where(Field.semester__course_id, courseId);
        q.Where(Field.request__stu_id, stuId);
        return q.Count(conn) > 0;
    }
}

/* EOF */
