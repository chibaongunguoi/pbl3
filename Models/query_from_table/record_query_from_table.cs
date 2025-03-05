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
    public static List<T> get_all_records(Table table)
    {
        Query q = new(table);
        return q.select<T>();
    }

    // ------------------------------------------------------------------------
    public static List<T> get_record_by_id(Table table, Field id_field, int id)
    {
        Query q = new(table);
        q.where_(id_field, id);
        return q.select<T>();
    }

    // ------------------------------------------------------------------------
    public static List<T> get_account_by_username_password(
        Table table,
        Field username_field,
        Field password_field,
        string username,
        string password
    )
    {
        Query q = new(table);
        q.where_(username_field, username);
        q.where_(password_field, password);
        return q.select<T>();
    }

    // ========================================================================
}

/* EOF */
