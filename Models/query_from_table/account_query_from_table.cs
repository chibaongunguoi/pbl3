class AccountQueryFromTable<T>
    where T : DataObj, new()
{
    // ========================================================================
    public static List<T> get_account_by_username_password(
        string table_name,
        string username,
        string password
    )
    {
        var query_creator = new QueryCreator(table_name);
        query_creator.add_string_condition("username", username);
        query_creator.add_string_condition("password", password);
        return query_creator.exec_select_query<T>();
    }

    // ------------------------------------------------------------------------
    public static List<T> get_account_by_username(string table_name, string username)
    {
        var query_creator = new QueryCreator(table_name);
        query_creator.add_string_condition("username", username);
        return query_creator.exec_select_query<T>();
    }

    // ========================================================================
}

/* EOF */
