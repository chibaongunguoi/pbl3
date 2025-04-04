using Microsoft.Data.SqlClient;

struct BriefTeacherCard
{
    public int id;
    public string name;
    public string gender;
    public string bday;
    public string description;

    // ------------------------------------------------------------------------
    public static List<BriefTeacherCard> get_page(
        SqlConnection conn,
        int page = 1,
        int num_objs = 20
    )
    {
        List<BriefTeacherCard> cards = new();
        void func(SqlDataReader reader)
        {
            int pos = 0;
            Teacher teacher = DataReader.get_data_obj<Teacher>(reader, ref pos);
            BriefTeacherCard card = new()
            {
                id = teacher.id,
                name = teacher.name,
                gender = IoUtils.conv(teacher.gender),
                bday = IoUtils.conv(teacher.bday),
                description = teacher.description,
            };

            cards.Add(card);
        }
        Query q = new(Table.teacher);
        q.offset(page, num_objs);
        q.select(conn, func);
        return cards;
    }
}

struct DetailedTeacherCard
{
    public int id;
    public string name;
    public string bday;
    public string grades;
    public string subjects;
    public string description;
}
