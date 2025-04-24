using Microsoft.Data.SqlClient;

struct RatingCard
{
    public string name;
    public int score;
    public string date;
    public string semester_dates;
    public string description;

    public static Query get_query_creator()
    {
        Query q = new(Tbl.rating);
        q.Join(Field.semester__id, Field.rating__semester_id);
        q.Join(Field.student__id, Field.rating__stu_id);
        q.Output(Field.student__name);
        q.Output(Field.rating__stars);
        q.Output(Field.rating__timestamp);
        q.Output(Field.semester__start_date);
        q.Output(Field.semester__finish_date);
        q.Output(Field.rating__description);
        return q;
    }

    public static RatingCard get_card(SqlConnection conn, SqlDataReader reader)
    {
        int pos = 0;
        string name = QDataReader.GetString(reader, ref pos);
        int score = QDataReader.GetInt(reader, ref pos);
        DateTime date = QDataReader.GetDateTime(reader, ref pos);
        DateOnly start_date = QDataReader.GetDateOnly(reader, ref pos);
        DateOnly finish_date = QDataReader.GetDateOnly(reader, ref pos);
        string descrip = QDataReader.GetString(reader, ref pos);

        RatingCard ra = new()
        {
            name = name,
            score = score,
            date = IoUtils.conv(date),
            semester_dates = $"{IoUtils.conv(start_date)} - {IoUtils.conv(finish_date)}",
            description = descrip,
        };
        return ra;
    }
}
