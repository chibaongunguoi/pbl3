class MultiAccountQueryFromTable<T>
    where T : DataObj, new()
{
    // ========================================================================
    public static List<T> get_all_accounts(string table_name)
    {
        var query_creator = new QueryCreator<T>(table_name);
        return query_creator.exec_query();
    }

    // ========================================================================
}

/* EOF */
