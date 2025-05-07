using Microsoft.Data.SqlClient;

class DetailedTeacherCard
{
    public int id;
    public string name = "";
    public string gender = "";
    public string bday = "";

    public string Tel = "";
    public string description = "";

    public static Query getQueryCreator()
    {
        Query q = new(Tbl.teacher);
        q.Output(Field.teacher__id);
        q.Output(Field.teacher__name);
        q.Output(Field.teacher__gender);
        q.Output(Field.teacher__bday);
        q.Output(Field.teacher__tel);
        q.Output(Field.teacher__description);
        return q;
    }

    public static DetailedTeacherCard getCard(SqlDataReader reader)
    {
        int pos = 0;
        var id = QDataReader.GetInt(reader, ref pos);
        var name = QDataReader.GetString(reader, ref pos);
        var gender = QDataReader.GetString(reader, ref pos);
        var bday = QDataReader.GetDateOnly(reader, ref pos);
        var tel = QDataReader.GetString(reader, ref pos);
        var description = QDataReader.GetString(reader, ref pos);
        DetailedTeacherCard card = new()
        {
            id = id,
            name = name,
            gender = IoUtils.convGender(gender),
            bday = IoUtils.conv(bday),
            Tel = tel,
            description = description,
        };
        return card;
    }
}

/* EOF */
