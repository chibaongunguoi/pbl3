using Microsoft.Data.SqlClient;

public class AdminMngStuCard
{
    public int TableIndex { get; set; }
    public int StuId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public string BDay { get; set; } = string.Empty;
    public string Tel { get; set; } = string.Empty;

    public static Query GetQuery()
    {
        Query q = new(Tbl.student);
        q.Output(Field.student__id);
        q.Output(Field.student__name);
        q.Output(Field.student__gender);
        q.Output(Field.student__bday);
        q.Output(Field.student__tel);
        return q;
    }

    public static AdminMngStuCard GetCard(SqlDataReader reader, ref int tableIndex)
    {
        int pos = 0;
        AdminMngStuCard card = new()
        {
            TableIndex = tableIndex++,
            StuId = QDataReader.GetInt(reader, ref pos),
            Name = QDataReader.GetString(reader, ref pos),
            Gender = QDataReader.GetString(reader, ref pos),
            BDay = IoUtils.conv(QDataReader.GetDateOnly(reader, ref pos)),
            Tel = QDataReader.GetString(reader, ref pos)
        };
        return card;
    }
}