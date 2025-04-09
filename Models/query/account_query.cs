using Microsoft.Data.SqlClient;

sealed class AccountQuery<T>
    where T : DataObj, new()
{
    // ========================================================================
    public delegate List<T> QueryFunction(Table table);

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
    public static Table any(QueryFunction f, out List<T> result)
    {
        result = new();
        Table latest_table = Table.none;
        foreach (var table in TableMngr.get_account_tables())
        {
            result = f(table);
            if (result.Count > 0)
            {
                latest_table = table;
                break;
            }
        }

        return latest_table;
    }

    // ========================================================================
    public static List<T> get_all_accounts(SqlConnection conn)
    {
        return all(table => CommonQuery<T>.get_all_records(conn, table));
    }

    // ========================================================================
    public static Table get_account_by_id(SqlConnection conn, int id, out List<T> result)
    {
        return any(table => CommonQuery<T>.get_record_by_id(conn, id, table), out result);
    }

    // ------------------------------------------------------------------------
    public static Table get_account_by_username_password(
        SqlConnection conn,
        string username,
        string password,
        out List<T> result,
        Table? table = null
    )
    {
        List<T> func(Table table)
        {
            var q = new Query(table);
            q.where_(QueryUtils.cat(table, FieldSuffix.username), username);
            q.where_(QueryUtils.cat(table, FieldSuffix.password), password);
            return q.select<T>(conn);
        }
        if (!table.HasValue)
        {
            return any(table => func(table), out result);
        }

        result = func(table.Value);
        return Table.none;
    }

    // ========================================================================
}

/* EOF */
