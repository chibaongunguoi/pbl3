using Microsoft.Data.SqlClient;

sealed class AccountQuery<T>
    where T : DataObj, new()
{
    // ========================================================================
    public delegate List<T> QueryFunction(string table);

    // ------------------------------------------------------------------------
    // INFO: Tìm kiếm trên tất cả các bảng tài khoản.
    // Lấy kết quả từ tất cả các bảng.
    public static List<T> all(QueryFunction f)
    {
        List<T> result = new List<T>();
        foreach (var table in new List<string> { Tbl.student, Tbl.teacher, Tbl.admin })
        {
            result.AddRange(f(table));
        }

        return result;
    }

    // ------------------------------------------------------------------------
    // INFO: Tìm kiếm trên tất cả các bảng tài khoản.
    // Khi có kết quả thì dừng việc tìm kiếm.
    public static string any(QueryFunction f, out List<T> result)
    {
        result = new();
        string latest_table = Tbl.none;
        foreach (var table in new List<string> { Tbl.student, Tbl.teacher, Tbl.admin })
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
    public static string get_account_by_id(SqlConnection conn, int id, out List<T> result)
    {
        return any(table => CommonQuery<T>.get_record_by_id(conn, table, id), out result);
    }

    // ------------------------------------------------------------------------
    public static string get_account_by_username_password(
        SqlConnection conn,
        string username,
        string password,
        out List<T> result,
        string? table = null
    )
    {
        List<T> func(string table)
        {
            Query q = new(table);
            q.Where(Fld.username, username);
            q.Where(Fld.password, password);
            return q.select<T>(conn);
        }
        if (string.IsNullOrEmpty(table))
        {
            return any(func, out result);
        }

        result = func(table);
        return Tbl.none;
    }

    // ========================================================================
}

/* EOF */
