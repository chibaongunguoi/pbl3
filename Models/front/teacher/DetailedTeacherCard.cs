using Microsoft.Data.SqlClient;

class DetailedTeacherCard
{
    public int id;
    public string name = "";
    public string gender = "";
    public string bday = "";
    public string description = "";

    public static Query getQueryCreator()
    {
        Query q = new(Tbl.teacher);
        q.output(Field.teacher__id);
        q.output(Field.teacher__name);
        q.output(Field.teacher__gender);
        q.output(Field.teacher__bday);
        q.output(Field.teacher__description);
        return q;
    }

    public static DetailedTeacherCard getCard(SqlDataReader reader)
    {
        int pos = 0;
        var id = DataReader.getInt(reader, ref pos);
        var name = DataReader.getStr(reader, ref pos);
        var gender = DataReader.getStr(reader, ref pos);
        var bday = DataReader.getDate(reader, ref pos);
        var description = DataReader.getStr(reader, ref pos);
        DetailedTeacherCard card = new()
        {
            id = id,
            name = name,
            gender = IoUtils.convGender(gender),
            bday = IoUtils.conv(bday),
            description = description,
        };
        return card;
    }
}

/* EOF */
