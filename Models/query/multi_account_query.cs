class MultiAccountQuery<T>
    where T : DataObj, new()
{
    // ========================================================================
    public List<T> get_all_accounts()
    {
        MultiAccountQueryResult<T> query_handler = new MultiAccountQueryResult<T>();
        query_handler.exec_function(table_name =>
            MultiAccountQueryFromTable<T>.get_all_accounts(table_name)
        );
        return query_handler.result;
    }

    // ========================================================================
}

/* EOF */
