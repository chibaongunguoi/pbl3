using Microsoft.Data.SqlClient;

class DetailedCourseCard : BriefCourseCard
{
    public string description = "";

    public new static Query GetQueryCreator()
    {
        Query q = BriefCourseCard.GetQueryCreator();
        q.Output(Field.semester__description);
        return q;
    }

    public override void Fetch(SqlDataReader reader, ref int pos)
    {
        base.Fetch(reader, ref pos);
        description = QDataReader.GetString(reader, ref pos);
    }
}

/* EOF */
