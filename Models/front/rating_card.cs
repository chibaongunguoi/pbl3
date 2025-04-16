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
        q.join(Tbl.student, Fld.id, Tbl.rating, Fld.stu_id);

        q.output(Tbl.student, Fld.name);
        q.output(Tbl.rating, Fld.stars);
        q.output(Tbl.rating, Fld.date);
        q.output(Tbl.rating, Fld.description);
        return q;
    }

    public static RatingCard get_card(SqlConnection conn, SqlDataReader reader)
    {
        int pos = 0;
        string name = DataReader.get_string(reader, ref pos);
        int score = DataReader.get_int(reader, ref pos);
        var date = DataReader.get_date(reader, ref pos);
        string descrip = DataReader.get_string(reader, ref pos);

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
