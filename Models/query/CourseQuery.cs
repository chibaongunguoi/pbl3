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
        q2.join(Field.semester__id, Field.rating__semester_id);
        q2.Where(Field.semester__course_id, course_id);
        q2.outputAvgCastFloat(Field.rating__stars);
        q.outputQuery(q2.selectQuery());

        // rating count
        q2 = new(Tbl.rating);
        q2.join(Field.semester__id, Field.rating__semester_id);
        q2.Where(Field.semester__course_id, course_id);
        q2.output(QPiece.countAll);
        q.outputQuery(q2.selectQuery());

        double temp_avg_rating = 0;
        int temp_num_ratings = 0;

        q.select(
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
}

/* EOF */
