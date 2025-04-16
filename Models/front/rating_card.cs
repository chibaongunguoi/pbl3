using Microsoft.Data.SqlClient;

struct RatingCard
{
    public string name;
    public int score;
    public string date;
    public string description;

    public static Query get_query_creator()
    {
        Query q = new(Tbl.rating);
        q.join(Field.student__id, Field.rating__stu_id);
        q.output(Field.student__name);
        q.output(Field.rating__stars);
        q.output(Field.rating__date);
        q.output(Field.rating__description);
        return q;
    }

    public static RatingCard get_card(SqlConnection conn, SqlDataReader reader)
    {
        int pos = 0;
        string name = DataReader.getStr(reader, ref pos);
        int score = DataReader.getInt(reader, ref pos);
        var date = DataReader.getDate(reader, ref pos);
        string descrip = DataReader.getStr(reader, ref pos);

        RatingCard ra = new()
        {
            name = name,
            score = score,
            date = IoUtils.conv(date),
            description = descrip,
        };
        return ra;
    }
}
