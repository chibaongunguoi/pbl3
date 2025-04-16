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
        q.join(Field.subject__id, Field.course__sbj_id);
        q.output(Field.course__id);
        q.output(Field.course__name);
        q.output(Field.course__state);
        q.output(Field.subject__name);
        q.output(Field.subject__grade);
        string local_alias = "local_alias";
        Query q2 = new(QPiece.alias(Tbl.rating, local_alias));
        q2.Where(local_alias, Fld.course_id, Tbl.course, Fld.id);
        q2.outputClause(QPiece.avg(QPiece.castFloat(Fld.stars)));
        q.outputQuery(q2.selectQuery());

        q2 = new(QPiece.alias(Tbl.rating, local_alias));
        q2.Where(local_alias, Fld.course_id, Tbl.course, Fld.id);
        q2.outputClause(QPiece.countAll);
        q.outputQuery(q2.selectQuery());
        return q;
    }

    public static ManageCourseCard get_card(SqlDataReader reader, ref int current_table_index)
    {
        int pos = 0;
        int course_id = DataReader.getInt(reader, ref pos);
        string course_name = DataReader.getStr(reader, ref pos);
        string course_state = DataReader.getStr(reader, ref pos);
        var subject = DataReader.getStr(reader, ref pos);
        var grade = DataReader.getInt(reader, ref pos);

        var avg_rating = DataReader.getDouble(reader, ref pos);
        int num_ratings = DataReader.getInt(reader, ref pos);
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
