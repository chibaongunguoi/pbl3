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
    public static void any(QueryFunction f, out List<T> result, ref string outTable)
    {
        result = new();
        foreach (var table in new List<string> { Tbl.student, Tbl.teacher, Tbl.admin })
        {
            result = f(table);
            if (result.Count > 0)
            {
                outTable = table;
                break;
            }
        }
    }

    // ========================================================================
    public static List<T> get_all_accounts(SqlConnection conn)
    {
        return all(table => CommonQuery<T>.get_all_records(conn, table));
    }

    // ========================================================================
    public static void get_account_by_id(SqlConnection conn, int id, out List<T> result, ref string outTable)
    {
        any(table => CommonQuery<T>.get_record_by_id(conn, table, id), out result, ref outTable);
    }

    // ------------------------------------------------------------------------
    public static void getAccountByUsernamePassword(
        SqlConnection conn,
        string username,
        string password,
        out List<T> result,
        ref string outTable,
        string? table = null
    )
    {
        List<T> func(string table)
        {
            List<T> result = new();
            Query q = new(table);
            q.Where(Fld.username, username);
            q.Where(Fld.password, password);
            q.Select(conn, reader => result.Add(QDataReader.GetDataObj<T>(reader)));
            return result;
        }
        if (string.IsNullOrEmpty(table))
        {
            any(func, out result, ref outTable);
            return;
        }

        result = func(table);
    }

    // ========================================================================
}

/* EOF */
