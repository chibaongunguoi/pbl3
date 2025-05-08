using Microsoft.Data.SqlClient;

public class ManageRatingCard
{
    public int TableIndex { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public string StudentName { get; set; } = string.Empty;
    public int Score {get;set;}
    public string TimeStamp { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;

    public static Query GetQueryCreator()
    {
        Query q = new Query(Tbl.rating);
        q.Join(Field.student__id, Field.rating__stu_id);
        q.Join(Field.semester__id, Field.rating__semester_id);
        q.Join(Field.course__id, Field.semester__course_id);
        q.Output(Field.course__name);
        q.Output(Field.student__name);
        q.Output(Field.rating__stars);
        q.Output(Field.rating__timestamp);
        q.Output(Field.rating__description);
        return q;
    }
    public static ManageRatingCard GetCard(SqlDataReader reader, ref int tableIdx)
    {
        int pos = 0;
        ManageRatingCard card = new()
        {
            TableIndex = tableIdx++,
            CourseName = QDataReader.GetString(reader, ref pos),
            StudentName = QDataReader.GetString(reader, ref pos),
            Score = QDataReader.GetInt(reader, ref pos),
            TimeStamp = IoUtils.conv(QDataReader.GetDateTime(reader, ref pos)),
            Comment = QDataReader.GetString(reader, ref pos)
        };
        return card;
    }
}