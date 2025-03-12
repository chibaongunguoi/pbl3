class Test2
{
    // ------------------------------------------------------------------------
    public static void test() { }

    public static void test2()
    {
        List<Teacher> teachers = find_by_id("2007");
        foreach (Teacher teacher in teachers)
        {
            Console.WriteLine(teacher.ToString());
        }
    }

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
}

/* EOF */
