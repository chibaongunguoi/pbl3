class SingleAccountQueryFromTable
{
    // ========================================================================
    public static List<T> get_account_by_id<T>(string table_name, int id)
        where T : DataObj, new()
    {
        var query_creator = new QueryCreator<T>(table_name);
        query_creator.add_numeric_condition("id", id);
        return query_creator.exec_query();
    }

    // ========================================================================
    public static List<T> get_account_by_username_password<T>(
        string table_name,
        string username,
        string password
    )
        where T : DataObj, new()
    {
        var query_creator = new QueryCreator<T>(table_name);
        query_creator.add_string_condition("username", username);
        query_creator.add_string_condition("password", password);
        return query_creator.exec_query();
    }

    // ========================================================================
    public static List<T> get_account_by_username<T>(string table_name, string username)
        where T : DataObj, new()
    {
        var query_creator = new QueryCreator<T>(table_name);
        query_creator.add_string_condition("username", username);
        return query_creator.exec_query();
    }

    // ========================================================================
}

/* EOF */
