using Microsoft.Data.SqlClient;

// INFO: Tập hợp các hàm dùng để tạo đối tượng từ một SqlDataReader.
// Cần xác định pos là chỉ số cột bắt đầu để đọc dữ liệu.
sealed class DataReader
{
    // ========================================================================
    public static int get_int(SqlDataReader reader, ref int pos)
    {
        return reader.GetInt32(pos++);
    }

    // ------------------------------------------------------------------------
    public static string get_string(SqlDataReader reader, ref int pos)
    {
        return reader.GetString(pos++);
    }

    // ------------------------------------------------------------------------
    public static DateOnly get_date(SqlDataReader reader, ref int pos)
    {
        DateTime d = reader.GetDateTime(pos++);
        return new(d.Year, d.Month, d.Day);
    }

    // ------------------------------------------------------------------------
    public static T get_enum<T>(SqlDataReader reader, ref int pos)
        where T : Enum
    {
        return (T)Enum.ToObject(typeof(T), reader.GetInt32(pos++));
    }

    // ========================================================================
    public static T get_data_obj<T>(SqlDataReader reader, ref int pos)
        where T : DataObj, new()
    {
        T info = new T();
        info.fetch_data(reader, ref pos);
        return info;
    }

    // ========================================================================
    public static string get_sql_repr(SqlDataReader reader)
    {
        List<string> record = new();
        for (int i = 0; i < reader.FieldCount; i++)
        {
            record.Add(reader[i].ToString() ?? "");
        }

        return string.Join(",", record);
    }

    // ========================================================================
}

/* EOF */
