using Microsoft.Data.SqlClient;

public class CourseOption : DataObj
{
    public int id;
    public string name = "";

    public static Query get_query_creator()
    {
        Query q = new(Tbl.course);
        q.OutputClause(Field.course__id, Field.course__name);
        return q;
    }

    public override void Fetch(SqlDataReader reader, ref int pos)
    {
        base.Fetch(reader, ref pos);
        id = QDataReader.GetInt(reader, ref pos);
        string course_name = QDataReader.GetString(reader, ref pos);
        name = $"{id} - {course_name}";
    }
}

class AddSemesterPage
{
    public List<CourseOption> courses = [];

    public AddSemesterPage(string username)
    {
    }
}
