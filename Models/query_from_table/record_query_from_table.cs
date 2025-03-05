class RecordQueryFromTable<T>
    where T : DataObj, new()
{
    // ========================================================================
    public static List<T> get_all_records(string table_name)
    {
        var q = new RawQuery(table_name);
        return q.select<T>();
    }

    // ------------------------------------------------------------------------
    public static List<T> get_record_by_id(string table_name, int id)
    {
        var q = new RawQuery(table_name);
        q.where_("id", id);
        return q.select<T>();
    }

    // ========================================================================
}

/* EOF */
