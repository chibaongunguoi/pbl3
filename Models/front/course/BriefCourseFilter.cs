using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;

public class BriefCourseFilter
{
    public string? SubjectName { get; set; } = null;
    public int? Grade { get; set; } = null;
    public string? Gender { get; set; } = null;
    public string? CourseName { get; set; } = null;

    public List<string> SubjectNames { get; set; } =[];
    public List<int> Grades { get; set; } = [];

    public void Reset(SqlConnection conn)
    {
        Query q2 = new(Tbl.subject);
        q2.OutputDistinct(Field.subject__name);
        q2.GroupBy(Field.subject__name);
        q2.Select(conn, reader =>
        {
            string name = QDataReader.GetString(reader);
            SubjectNames.Add(name);
        });

        Query q3 = new(Tbl.subject);
        q3.OutputDistinct(Field.subject__grade);
        q3.GroupBy(Field.subject__grade);
        q3.Select(conn, reader =>
        {
            int grade = QDataReader.GetInt(reader);
            Grades.Add(grade);
        });
    }
}