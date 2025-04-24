using Microsoft.Data.SqlClient;

class CourseOption : DataObj
{
    public int id;
    public string name = "";

    public static Query get_query_creator()
    {
        Query q = new(Tbl.course);
        q.OutputClause(Fld.id, Fld.name);
        return q;
    }

    public override void fetch(SqlDataReader reader, ref int pos)
    {
        base.fetch(reader, ref pos);
        id = QDataReader.GetInt(reader, ref pos);
        string course_name = QDataReader.GetString(reader, ref pos);
        name = $"{id} - {course_name}";
    }
}

class AddSemesterPage
{
    public List<CourseOption> courses = [];

    public AddSemesterPage(int tch_id)
    {
        QDatabase.Exec(
            delegate(SqlConnection conn)
            {
                Query q = CourseOption.get_query_creator();
                q.Where(Field.course__status, CourseStatus.finished);
                q.Where(Field.course__tch_id, tch_id);
                q.Select(
                    conn,
                    reader => this.courses.Add(QDataReader.GetDataObj<CourseOption>(reader))
                );
            }
        );
    }
}
