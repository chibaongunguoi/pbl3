using Microsoft.Data.SqlClient;

struct BriefTeacherCard
{
    public int id;
    public string name;
    public string gender;
    public string bday;
    public string description;

    public static BriefTeacherCard get_card(SqlConnection conn, SqlDataReader reader)
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

        return card;
    }

    // ------------------------------------------------------------------------
    public static List<BriefTeacherCard> get_page(
        SqlConnection conn,
        int page = 1,
        int num_objs = 20
    )
    {
        List<BriefTeacherCard> cards = new();
        Query q = new(Table.teacher);
        q.offset(page, num_objs);
        q.select(conn, reader => cards.Add(get_card(conn, reader)));
        return cards;
    }

    // ------------------------------------------------------------------------
    public static List<BriefTeacherCard> get_by_id(SqlConnection conn, int tch_id)
    {
        List<BriefTeacherCard> cards = new();
        Query q = new(Table.teacher);
        q.where_(Field.teacher__id, tch_id);
        q.select(conn, reader => cards.Add(get_card(conn, reader)));
        return cards;
    }

    // ------------------------------------------------------------------------
}
