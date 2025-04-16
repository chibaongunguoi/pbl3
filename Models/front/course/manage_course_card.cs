using Microsoft.Data.SqlClient;

class ManageCourseCard
{
    public int table_index;
    public int course_id;
    public string course_name = "";
    public string course_state = "";
    public string avg_rating = "";
    public string subject = "";
    public int grade;

    public static Query get_query_creator()
    {
        Query q = new(Tbl.course);
        q.Join(Tbl.subject, Fld.id, Tbl.course, Fld.sbj_id);
        q.OutputClause(
            QPiece.dot(Tbl.course, Fld.id),
            QPiece.dot(Tbl.course, Fld.name),
            QPiece.dot(Tbl.course, Fld.state),
            QPiece.dot(Tbl.subject, Fld.name),
            QPiece.dot(Tbl.subject, Fld.grade)
        );
        string local_alias = "local_alias";
        Query q2 = new(QPiece.alias(Tbl.rating, local_alias));
        q2.Where(local_alias, Fld.course_id, Tbl.course, Fld.id);
        q2.OutputClause(QPiece.avg(QPiece.cast_float(Fld.stars)));
        q.OutputClause(QPiece.bracket(q2.SelectQuery()));

        q2 = new(QPiece.alias(Tbl.rating, local_alias));
        q2.Where(local_alias, Fld.course_id, Tbl.course, Fld.id);
        q2.OutputClause(QPiece.countAll);
        q.OutputClause(QPiece.bracket(q2.SelectQuery()));
        return q;
    }

    public static ManageCourseCard get_card(SqlDataReader reader, ref int current_table_index)
    {
        int pos = 0;
        int course_id = DataReader.get_int(reader, ref pos);
        string course_name = DataReader.get_string(reader, ref pos);
        string course_state = DataReader.get_string(reader, ref pos);
        var subject = DataReader.get_string(reader, ref pos);
        var grade = DataReader.get_int(reader, ref pos);

        var avg_rating = DataReader.get_double(reader, ref pos);
        int num_ratings = DataReader.get_int(reader, ref pos);
        string s_avg_rating = $"{Math.Round(avg_rating, 1)}/5 ({num_ratings})";
        ManageCourseCard card = new()
        {
            table_index = current_table_index++,
            course_id = course_id,
            course_name = course_name,
            course_state = course_state == CourseState.waiting ? "Đang diễn ra" : "Đã kết thúc",
            subject = subject,
            grade = grade,
            avg_rating = s_avg_rating,
        };

        return card;
    }
}
