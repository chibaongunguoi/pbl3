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
    private static void test_2(SqlConnection conn, SqlDataReader reader)
    {
        string tch_name = DataReader.get_string(reader, 0);
        string sbj_name = DataReader.get_string(reader, 1);
        Console.WriteLine("The teacher " + tch_name + " teaches " + sbj_name);
    }

    // ------------------------------------------------------------------------
    private static void test()
    {
        QueryCreator qc = new(TeacherSubjectQuery.get_table_name());
        qc.add_output_field(TeacherQuery.field("fullname"));
        qc.add_output_field(SubjectQuery.field("name"));
        qc.add_inner_join(
            TeacherQuery.get_table_name(),
            TeacherQuery.field("id"),
            TeacherSubjectQuery.field("tch_id")
        );
        qc.add_inner_join(
            SubjectQuery.get_table_name(),
            SubjectQuery.field("id"),
            TeacherSubjectQuery.field("sbj_id")
        );
        qc.exec_select_query(test_2);
    }

    // ========================================================================
}

/* EOF */
