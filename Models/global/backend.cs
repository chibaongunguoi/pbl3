namespace module_backend;

using module_config;
using module_data;
using module_object;
using module_query;

sealed class Backend
{
    // ========================================================================
    public static void start()
    {
        try
        {
            Config.load();
            test();
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: " + e.Message);
        }
    }

    // ------------------------------------------------------------------------
    private static void test()
    {
        if (Boolean.Parse(Config.get_config("data", "data_generator")))
        {
            DataGenerator.generate();
        }

        List<DemoUser> demo_users = DemoUserQuery.get_all_demo_users();
        List<Student> students = Database.fetch_data_new_conn<Student>("SELECT * FROM TblStudent");
        Console.WriteLine("DemoUsers:");
        // foreach (DemoUser demo_user in demo_users)
        // {
        //     Console.WriteLine(demo_user.ToString());
        // }
        foreach (Student student in students)
        {
            Console.WriteLine(student.ToString());
        }
    }

    // ========================================================================
}

/* EOF */
