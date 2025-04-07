using Microsoft.Data.SqlClient;

struct BriefTeacherCard
{
    public int id;
    public string name;
    public string gender;
    public string bday;
    public string description;

    static void get_card(SqlDataReader reader, ref List<BriefTeacherCard> cards)
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
        q.select(conn, reader => get_card(reader, ref cards));
        return cards;
    }

    // ------------------------------------------------------------------------
    public static List<BriefTeacherCard> get_by_id(SqlConnection conn, int tch_id)
    {
        List<BriefTeacherCard> cards = new();
        Query q = new(Table.teacher);
        q.where_(Field.teacher__id, tch_id);
        q.select(conn, reader => get_card(reader, ref cards));
        return cards;
    }

    // ------------------------------------------------------------------------
}
