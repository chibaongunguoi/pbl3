using Microsoft.Data.SqlClient;

public class AdminSemesterStudentCard
{
    public int TableIndex { get; set; }
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public DateTime RegistrationDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public string Tel { get; set; } = string.Empty;    public static Query GetQuery(int semesterId)
    {
        Query q = new(Tbl.request);
        q.Where(Field.request__semester_id, semesterId);
        q.Output(Field.request__stu_id);
        q.Output(Field.request__timestamp);
        q.Output(Field.request__status);
        
        // Join with student to get student information
        q.Join(Field.student__id, Field.request__stu_id);
        q.Output(Field.student__name);
        q.Output(Field.student__gender);
        q.Output(Field.student__tel);
        
        // Order by timestamp date
        q.OrderBy(Field.request__timestamp, desc: true);
        
        return q;
    }    public static AdminSemesterStudentCard GetCard(SqlDataReader reader, ref int tableIndex)
    {
        int pos = 0;
        AdminSemesterStudentCard card = new()
        {
            TableIndex = tableIndex++,
            StudentId = QDataReader.GetInt(reader, ref pos),
            RegistrationDate = QDataReader.GetDateTime(reader, ref pos),
            Status = QDataReader.GetString(reader, ref pos),
            StudentName = QDataReader.GetString(reader, ref pos),
            Gender = QDataReader.GetString(reader, ref pos),
            Tel = QDataReader.GetString(reader, ref pos)
        };
        return card;
    }
}
