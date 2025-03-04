class RecordQueryFromTable<T>
    where T : DataObj, new()
{
    // ========================================================================
    public static List<T> get_all_records(string table_name)
    {
        var query_creator = new QueryCreator(table_name);
        return query_creator.exec_select_query<T>();
    }

    // ------------------------------------------------------------------------
    public static List<T> get_record_by_id(string table_name, int id)
    {
        var query_creator = new QueryCreator(table_name);
        query_creator.add_numeric_condition("id", id);
        return query_creator.exec_select_query<T>();
    }

    // ========================================================================
}

/* EOF */
