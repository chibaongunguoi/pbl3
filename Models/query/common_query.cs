class CommonQuery { }

class CommonQuery<T>
    where T : DataObj, new()
{
    // ========================================================================
    public static List<T> get_all_records(Table table)
    {
        Query q = new(table);
        return q.select<T>();
    }

    // ------------------------------------------------------------------------
    public static List<T> get_record_by_id(Table table, int id)
    {
        var q = new Query(table);
        q.where_(QueryUtils.cat(table, FieldSuffix.id), id);
        return q.select<T>();
    }

    // ========================================================================
    // INFO: Tìm kiếm tài khoản bằng tên đăng nhập và mật khẩu
    public static List<T> get_account_by_username_password(
        Table table,
        string username,
        string password
    )
    {
        var q = new Query(table);
        q.where_(QueryUtils.cat(table, FieldSuffix.username), username);
        q.where_(QueryUtils.cat(table, FieldSuffix.password), password);
        return q.select<T>();
    }

    // ------------------------------------------------------------------------
    // INFO: Tìm kiếm tài khoản chỉ bằng tên đăng nhập
    // (thường dùng để kiểm tra tài khoản tồn tại hay không)
    public static List<T> get_account_by_username(Table table, string username)
    {
        var q = new Query(table);
        q.where_(QueryUtils.cat(table, FieldSuffix.username), username);
        return q.select<T>();
    }

    // ========================================================================
}


/* EOF */
