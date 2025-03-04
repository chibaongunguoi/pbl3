class ConfigUtils
{
    // ========================================================================
    private static Dictionary<InfoAccountType, string> s_table_name_dict =
        new Dictionary<InfoAccountType, string>();

    public static string s_demo_user_table_name = "";

    // ========================================================================
    public static void load()
    {
        s_table_name_dict[InfoAccountType.STUDENT] = ConfigJson.get_table_name("student");
        s_table_name_dict[InfoAccountType.TEACHER] = ConfigJson.get_table_name("teacher");
        s_demo_user_table_name = ConfigJson.get_table_name("demo_user");
    }

    // ========================================================================
    public static string get_table_name(InfoAccountType account_type)
    {
        if (!s_table_name_dict.ContainsKey(account_type))
            return "";
        return s_table_name_dict[account_type];
    }

    // ------------------------------------------------------------------------
    public static Dictionary<InfoAccountType, string> get_table_name_dict()
    {
        return s_table_name_dict;
    }

    // ========================================================================
}

/* EOF */
