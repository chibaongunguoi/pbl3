using Microsoft.Data.SqlClient;

sealed class Backend
{
    // ========================================================================
    public static void start()
    {
        try
        {
            ConfigOptionManager.load();
            DatabaseConfigManager.load();
            if (ConfigOptionManager.get_data_generator())
            {
                DataGenerator.generate();
            }

            test();
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: " + e.Message);
        }
    }

    // ------------------------------------------------------------------------
    private static void test_2(SqlDataReader reader, ref List<string> output)
    {
        string tch_name = DataReader.get_string(reader, 0);
        string sbj_name = DataReader.get_string(reader, 1);
        output.Add(tch_name + " - " + sbj_name);
    }

    // ------------------------------------------------------------------------
    private static void test()
    {
        List<string> output = new();
        Query q = new(Table.teacher_subject);
        q.join(Field.teacher__id, Field.teacher_subject__tch_id);
        q.join(Field.subject__id, Field.teacher_subject__sbj_id);
        q.output(Field.teacher__fullname);
        q.output(Field.subject__name);
        q.select(r => test_2(r, ref output));
        foreach (string s in output)
        {
            Console.WriteLine(s);
        }
    }

    // ========================================================================
}

/* EOF */
