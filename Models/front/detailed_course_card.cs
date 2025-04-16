using Microsoft.Data.SqlClient;

class DetailedCourseCard : BriefCourseCard
{
    public string description = "";

    public static new Query get_query_creator()
    {
        Query q = BriefCourseCard.get_query_creator();
        q.output(Tbl.semester, Fld.description);
        return q;
    }

    public override void fetch(SqlDataReader reader, ref int pos)
    {
        base.fetch(reader, ref pos);
        this.description = DataReader.getStr(reader, ref pos);
    }
}
