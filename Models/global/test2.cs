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
    public static List<BriefTeacherCard> demo2(SqlConnection conn)
    {
        List<BriefTeacherCard> cards = new();
        void func(SqlDataReader reader)
        {
            Teacher teacher = DataReader.get_data_obj<Teacher>(reader);
            BriefTeacherCard card = new()
            {
                id = teacher.id,
                name = teacher.name,
                gender = IoUtils.conv(teacher.gender),
                bday = IoUtils.conv(teacher.bday),
                description = teacher.description
            };

            cards.Add(card);
        }
        Query q = new(Table.teacher);
        q.select(conn, func);
        return cards;
    }

    public static List<BriefCourseCard> get_brief_course_cards(SqlConnection conn)
    {
        List<BriefCourseCard> cards = new();

        void get_card(SqlDataReader reader) {
            int pos = 0;
            BriefCourseCard card = new () {
                course_name = DataReader.get_string(reader, pos++),
                tch_name = DataReader.get_string(reader, pos++),
            subject = DataReader.get_string(reader, pos++),
            grade = DataReader.get_int(reader,pos++)
            };
            cards.Add(card);
         }

        Query q = new Query(Table.course);
        q.join(Field.subject__id, Field.course__sbj_id);
        q.join(Field.teacher__id, Field.course__tch_id);
        q.output(Field.course__name);
        q.output(Field.teacher__name);
        q.output(Field.subject__name);
        q.output(Field.subject__grade);
        return cards;
    }

    // ------------------------------------------------------------------------
}

/* EOF */
