using Microsoft.Data.SqlClient;

public class ManageStuSemCard
{
    public int TableIndex { get; set; }
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public string Bday { get; set; } = string.Empty;
    public string Tel { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;

    public static Query GetQueryCreator()
    {
        Query q = new(Tbl.request);
        q.Join(Field.student__id, Field.request__stu_id);
        q.Output(Field.student__id);
        q.Output(Field.student__name);
        q.Output(Field.student__gender);
        q.Output(Field.student__bday);
        q.Output(Field.student__tel);
        q.Output(Field.request__status);
        return q;
    }

    public static ManageStuSemCard GetCard(SqlDataReader reader, ref int tableIndex)
    {
        int pos = 0;
        ManageStuSemCard card = new()
        {
            TableIndex = tableIndex++,
            Id = QDataReader.GetInt(reader, ref pos),
            Name = QDataReader.GetString(reader, ref pos),
            Gender = QDataReader.GetString(reader, ref pos),
            Bday = IoUtils.conv(QDataReader.GetDateOnly(reader, ref pos)),
            Tel = QDataReader.GetString(reader, ref pos),
            Status = QDataReader.GetString(reader, ref pos)
        };

        return card;
    }
}