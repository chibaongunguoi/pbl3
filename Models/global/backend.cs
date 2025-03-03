sealed class Backend
{
    // ========================================================================
    public static void start()
    {
        try
        {
            Config.load();
            ConfigJson.load();
            ConfigUtils.load();
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

        List<Student> students = StudentQuery.get_all_students();
        Console.WriteLine("Students:");
        foreach (Student student in students)
        {
            Console.WriteLine(student.ToString());
        }
    }

    // ========================================================================
}

/* EOF */
