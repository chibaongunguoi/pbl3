sealed class SingleAccountQuery<T>
    where T : DataObj, new()
{
    // ========================================================================
    public static SingleAccountQueryResult<T> get_account_by_id(int id)
    {
        SingleAccountQueryResult<T> query_handler = new SingleAccountQueryResult<T>();
        query_handler.exec_function(table_name =>
            SingleAccountQueryFromTable.get_account_by_id<T>(table_name, id)
        );
        return query_handler;
    }

    // ========================================================================
    public static SingleAccountQueryResult<T> get_account_by_username_password(
        string username,
        string password
    )
    {
        SingleAccountQueryResult<T> query_handler = new SingleAccountQueryResult<T>();
        query_handler.exec_function(table_name =>
            SingleAccountQueryFromTable.get_account_by_username_password<T>(
                table_name,
                username,
                password
            )
        );
        return query_handler;
    }

    // ========================================================================
}

/* EOF */
