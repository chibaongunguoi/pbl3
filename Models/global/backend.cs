using Microsoft.Data.SqlClient;

sealed class Backend
{
    // ========================================================================
    public static void start()
    {
        try
        {
            ConfigOptionManager.load();
            TableMngr.load();
            if (ConfigOptionManager.get_data_generator())
            {
                DataGenerator.generate();
            }

            test2();
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: " + e.Message);
        }
    }

    // ------------------------------------------------------------------------
    private static void test()
    {
        List<string> output = new();
        void func(SqlDataReader reader)
        {
            string tch_name = DataReader.get_string(reader, 0);
            string sbj_name = DataReader.get_string(reader, 1);
            output.Add(tch_name + " - " + sbj_name);
        }
        Query q = new(Table.teacher_subject);
        q.join(Field.teacher__id, Field.teacher_subject__tch_id);
        q.join(Field.subject__id, Field.teacher_subject__sbj_id);
        q.output(Field.teacher__fullname);
        q.output(Field.subject__name);
        q.select(func);
        foreach (string s in output)
        {
            Console.WriteLine(s);
        }
    }

    // ------------------------------------------------------------------------
    private static void test2()
    {
        Query q = new(Table.student);
        q.where_(Field.student__id, 1007);
        foreach (var r in q.select<Student>())
        {
            Console.WriteLine(r.ToString());
        }
    }

    // ========================================================================
}

/* EOF */
