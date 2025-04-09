using Microsoft.Data.SqlClient;

sealed class AccountQuery<T>
    where T : DataObj, new()
{
    // ========================================================================
    private static Table s_latest_table;
    public delegate List<T> QueryFunction(Table table);

    // ------------------------------------------------------------------------
    public static Table get_latest_table() => s_latest_table;

    // ------------------------------------------------------------------------
    // INFO: Tìm kiếm trên tất cả các bảng tài khoản.
    // Lấy kết quả từ tất cả các bảng.
    public static List<T> all(QueryFunction f)
    {
        List<T> result = new List<T>();
        foreach (var table in TableMngr.get_account_tables())
        {
            result.AddRange(f(table));
        }

        return result;
    }

    // ------------------------------------------------------------------------
    // INFO: Tìm kiếm trên tất cả các bảng tài khoản.
    // Khi có kết quả thì dừng việc tìm kiếm.
    public static List<T> any(QueryFunction f)
    {
        List<T> result = new();
        foreach (var table in TableMngr.get_account_tables())
        {
            result = f(table);
            if (result.Count > 0)
            {
                s_latest_table = table;
                break;
            }
        }

        return result;
    }

    // ========================================================================
    public static List<T> get_all_accounts(SqlConnection conn)
    {
        return all(table => CommonQuery<T>.get_all_records(conn, table));
    }

    // ========================================================================
    public static List<T> get_account_by_id(SqlConnection conn, int id, Table? table = null)
    {
        if (!table.HasValue)
        {
            return any(table => CommonQuery<T>.get_record_by_id(conn, id, table));
        }

        return CommonQuery<T>.get_record_by_id(conn, id, table.Value);
    }

    // ------------------------------------------------------------------------
    public static List<T> get_account_by_username_password(
        SqlConnection conn,
        string username,
        string password,
        Table? table = null
    )
    {
        if (!table.HasValue)
        {
            return any(table => get_account_by_username_password(conn, username, password, table));
        }
        var q = new Query(table.Value);
        q.where_(QueryUtils.cat(table.Value, FieldSuffix.username), username);
        q.where_(QueryUtils.cat(table.Value, FieldSuffix.password), password);
        return q.select<T>(conn);
    }

    // ========================================================================
}

/* EOF */
