using Microsoft.Data.SqlClient;

class DetailedCourseCard : BriefCourseCard
{
    public string description = "";

    public static new Query get_query_creator()
    {
        Query q = BriefCourseCard.get_query_creator();
        q.OutputClause(QPiece.dot(Tbl.semester, Fld.description));
        return q;
    }

    public override void fetch_data(SqlDataReader reader, ref int pos)
    {
        base.fetch_data(reader, ref pos);
        this.description = DataReader.get_string(reader, ref pos);
    }
}
