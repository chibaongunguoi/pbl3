sealed class Backend
{
    // ========================================================================
    public static void start()
    {
        try
        {
            ConfigOptionManager.load();
            TableMngr.load();
            string server_name = ConfigOptionManager.get_server_name();
            string database_name = TableMngr.get_database_name();
            Database.init(server_name, database_name);
            if (ConfigOptionManager.get_data_generator())
            {
                DataGenerator.generate(server_name, database_name);
            }
            GeneralQuery.update_course_status();
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: " + e.Message);
        }
    }

    // ========================================================================
}

/* EOF */
