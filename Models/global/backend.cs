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
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: " + e.Message);
        }
    }

    // ========================================================================
}

/* EOF */
