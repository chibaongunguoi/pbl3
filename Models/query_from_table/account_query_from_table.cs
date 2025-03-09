// NOTE: Tìm kiếm tài khoản trên một bảng cụ thể.

class AccountQueryFromTable<T>
    where T : DataObj, new()
{
    // ========================================================================
    // INFO: Tìm kiếm tài khoản bằng tên đăng nhập và mật khẩu
    public static List<T> get_account_by_username_password(
        string table_name,
        string username,
        string password
    )
    {
        var q = new RawQuery(table_name);
        q.where_("username", username);
        q.where_("password", password);
        return q.select<T>();
    }

    // ------------------------------------------------------------------------
    // INFO: Tìm kiếm tài khoản chỉ bằng tên đăng nhập
    // (thường dùng để kiểm tra tài khoản tồn tại hay không)
    public static List<T> get_account_by_username(string table_name, string username)
    {
        var q = new RawQuery(table_name);
        q.where_("username", username);
        return q.select<T>();
    }

    // ========================================================================
}

/* EOF */
