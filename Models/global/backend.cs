namespace module_backend;

using module_config;
using module_object;
using module_query;

sealed class Backend
{
    // ========================================================================
    public static void start()
    {
        Config.load();
        test();
    }

    // ------------------------------------------------------------------------
    private static void test()
    {
        List<DemoUser> demo_users = DemoUserQuery.get_all_demo_users();
        Console.WriteLine("DemoUsers:");
        foreach (DemoUser demo_user in demo_users)
        {
            demo_user.print();
        }
    }

    // ========================================================================
}

/* EOF */
