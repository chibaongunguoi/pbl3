using Microsoft.Data.SqlClient;

class CourseOption : DataObj
{
    public int id;
    public string name = "";

    public static Query get_query_creator()
    {
        Query q = new(Table.course);
        q.output(Field.course__id);
        q.output(Field.course__name);
        return q;
    }

    public override void fetch_data(SqlDataReader reader, ref int pos)
    {
        base.fetch_data(reader, ref pos);
        id = DataReader.get_int(reader, ref pos);
        string course_name = DataReader.get_string(reader, ref pos);
        name = $"{id} - {course_name}";
    }
}

class AddSemesterPage
{
    public List<CourseOption> courses = new();

    public AddSemesterPage(int tch_id)
    {
        Database.exec(
            delegate(SqlConnection conn)
            {
                Query q = CourseOption.get_query_creator();
                q.where_(Field.course__tch_id, tch_id);
                q.where_(Field.course__state, CourseState.finished);
                this.courses = q.select<CourseOption>(conn);
            }
        );
    }
}
