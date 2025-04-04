using Microsoft.Data.SqlClient;

class Test2
{
    // ------------------------------------------------------------------------
    public static void test() => test3();

    // ------------------------------------------------------------------------
    public static void test2()
    {
        List<Teacher> teachers = find_by_id("2007");
        foreach (Teacher teacher in teachers)
        {
            Console.WriteLine(teacher.ToString());
        }
    }

    // ------------------------------------------------------------------------
    public static void test3() { }

    // ------------------------------------------------------------------------
    public static List<Teacher> find_by_id(string id_)
    {
        List<Teacher> teachers = new();
        if (!RegexPatterns.match(id_, RegexPatterns.numeric))
        {
            return teachers;
        }

        int id = int.Parse(id_);
        Query q = new(Table.teacher);
        q.where_(Field.teacher__id, id);
        teachers = Database.exec_list(conn => q.select<Teacher>(conn));
        return teachers;
    }

    // ------------------------------------------------------------------------
    public static List<BriefTeacherCard> get_brief_teacher_cards(
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

    // ------------------------------------------------------------------------
}

/* EOF */
