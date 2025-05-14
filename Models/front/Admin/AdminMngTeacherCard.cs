using Microsoft.Data.SqlClient;

public class AdminMngTeacherCard
{
    public int TableIndex { get; set; }
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Gender { get; set; } = "";
    public string Bday { get; set; } = "";
    public string Tel { get; set; } = "";
    public int SemesterCount { get; set; }
    public double Rating { get; set; }

    public static Query GetQuery()
    {
        // Base query that gets teacher information
        Query q = new(Tbl.teacher);
        q.Output(Field.teacher__id);
        q.Output(Field.teacher__name);
        q.Output(Field.teacher__gender);
        q.Output(Field.teacher__bday);
        q.Output(Field.teacher__tel);
        
        // Subquery to get semester count
        Query semesterCountQuery = new(Tbl.course);
        semesterCountQuery.Join(Field.semester__course_id, Field.course__id);
        semesterCountQuery.WhereField(Field.course__tch_id, Field.teacher__id);
        semesterCountQuery.Output(QPiece.countAll);
        q.OutputQuery(semesterCountQuery.SelectQuery());
        
        // Subquery to get average rating
        Query avgRatingQuery = new(Tbl.rating);
        avgRatingQuery.Join(Field.semester__id, Field.rating__semester_id);
        avgRatingQuery.Join(Field.course__id, Field.semester__course_id);
        avgRatingQuery.WhereField(Field.course__tch_id, Field.teacher__id);
        avgRatingQuery.OutputAvgCastFloat(Field.rating__stars);
        q.OutputQuery(avgRatingQuery.SelectQuery());
        
        return q;
    }

    public static AdminMngTeacherCard GetCard(SqlDataReader reader, ref int tableIndex)
    {
        int pos = 0;
        AdminMngTeacherCard card = new()
        {
            TableIndex = tableIndex++,
            Id = QDataReader.GetInt(reader, ref pos),
            Name = QDataReader.GetString(reader, ref pos),
            Gender = IoUtils.convGender(QDataReader.GetString(reader, ref pos)),
            Bday = IoUtils.conv(QDataReader.GetDateOnly(reader, ref pos)),
            Tel = QDataReader.GetString(reader, ref pos),
            SemesterCount = QDataReader.GetInt(reader, ref pos),
            Rating = QDataReader.GetDouble(reader, ref pos)
        };
        return card;
    }
}
