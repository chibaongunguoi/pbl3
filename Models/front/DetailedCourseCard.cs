using Microsoft.Data.SqlClient;

class DetailedCourseCard : BriefCourseCard
{
    public string description = "";

    public static new Query getQueryCreator()
    {
        Query q = BriefCourseCard.getQueryCreator();
        q.Output(Field.semester__description);
        return q;
    }

    public override void fetch(SqlDataReader reader, ref int pos)
    {
        base.fetch(reader, ref pos);
        this.description = DataReader.getStr(reader, ref pos);
    }
}

/* EOF */
