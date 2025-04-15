using Microsoft.Data.SqlClient;

class DetailedCourseCard : BriefCourseCard
{
    public string description = "";

    public static new QueryCreator get_query_creator()
    {
        QueryCreator q = BriefCourseCard.get_query_creator();
        q.output(QPiece.dot(Tbl.semester, Fld.description));
        return q;
    }

    public override void fetch_data(SqlDataReader reader, ref int pos)
    {
        base.fetch_data(reader, ref pos);
        this.description = DataReader.get_string(reader, ref pos);
    }
}
