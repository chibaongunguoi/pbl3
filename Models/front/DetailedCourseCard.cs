using Microsoft.Data.SqlClient;

class DetailedCourseCard : BriefCourseCard
{
    public string description = "";

    public static Query getQueryCreator()
    {
        Query q = BriefCourseCard.GetQueryCreator();
        q.Output(Field.semester__description);
        return q;
    }

    public override void Fetch(SqlDataReader reader, ref int pos)
    {
        base.Fetch(reader, ref pos);
        this.description = QDataReader.GetString(reader, ref pos);
    }
}

/* EOF */
