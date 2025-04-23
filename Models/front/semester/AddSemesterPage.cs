using Microsoft.Data.SqlClient;

class CourseOption : DataObj
{
    public int id;
    public string name = "";

    public static Query get_query_creator()
    {
        Query q = new(Tbl.course);
        q.outputClause(Fld.id, Fld.name);
        return q;
    }

    public override void fetch(SqlDataReader reader, ref int pos)
    {
        base.fetch(reader, ref pos);
        id = DataReader.getInt(reader, ref pos);
        string course_name = DataReader.getStr(reader, ref pos);
        name = $"{id} - {course_name}";
    }
}

class AddSemesterPage
{
    public List<CourseOption> courses = [];

    public AddSemesterPage(int tch_id)
    {
        QDatabase.exec(
            delegate(SqlConnection conn)
            {
                Query q = CourseOption.get_query_creator();
                q.Where(Field.course__status, CourseStatus.finished);
                q.Where(Field.course__tch_id, tch_id);
                q.Select(
                    conn,
                    reader => this.courses.Add(DataReader.getDataObj<CourseOption>(reader))
                );
            }
        );
    }
}
