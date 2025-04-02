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
    public static List<Dictionary<string, string>> demo(SqlConnection conn)
    {
        List<Dictionary<string, string>> teacher_dicts = new();
        void func(SqlDataReader reader)
        {
            Teacher teacher = DataReader.get_data_obj<Teacher>(reader);
            Dictionary<string, string> dict = teacher.to_dict();
            teacher_dicts.Add(dict);
        }

        void func2(SqlConnection conn)
        {
            Query q = new(Table.teacher);
            q.select(conn, func);
            for (int i = 0; i < teacher_dicts.Count; i++)
            {
                Query q2 = new(Table.teacher_subject);
                q2.join(Field.subject__id, Field.teacher_subject__sbj_id);
                q2.where_(Field.teacher_subject__tch_id, int.Parse(teacher_dicts[i]["id"]));
                q2.output(Field.subject__name);
                List<string> subjects = q2.select(conn);
                teacher_dicts[i]["subjects"] = string.Join(", ", subjects);
            }
        }

        Database.exec(func2);
        return teacher_dicts;
    }

    // ------------------------------------------------------------------------
    public static List<BriefTeacherCard> demo2(SqlConnection conn)
    {
        List<BriefTeacherCard> cards = new();
        void func(SqlDataReader reader)
        {
            Teacher teacher = DataReader.get_data_obj<Teacher>(reader);
            BriefTeacherCard card = new()
            {
                id = teacher.id,
                name = teacher.fullname,
                gender = IoUtils.conv(teacher.gender),
                bday = IoUtils.conv(teacher.bday),
            };

            cards.Add(card);
        }
        Query q = new(Table.teacher);
        q.select(conn, func);
        return cards;
    }
    // ------------------------------------------------------------------------
}

/* EOF */
