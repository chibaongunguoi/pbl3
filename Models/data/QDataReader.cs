using Microsoft.Data.SqlClient;

// INFO: Tập hợp các hàm dùng để tạo đối tượng từ một SqlDataReader.
// Cần xác định pos là chỉ số cột bắt đầu để đọc dữ liệu.
sealed class QDataReader
{
    // ========================================================================
    public static int GetInt(SqlDataReader reader, ref int pos)
    {
        return reader.GetInt32(pos++);
    }

    public static int GetInt(SqlDataReader reader, int pos = 0)
    {
        return GetInt(reader, ref pos);
    }

    public static double GetDouble(SqlDataReader reader, ref int pos)
    {
        return reader.GetDouble(pos++);
    }

    public static double GetDouble(SqlDataReader reader, int pos = 0)
    {
        return GetDouble(reader, ref pos);
    }

    // ------------------------------------------------------------------------
    public static string GetString(SqlDataReader reader, int pos = 0)
    {
        return reader.GetString(pos++);
    }

    public static string GetString(SqlDataReader reader, ref int pos)
    {
        return reader.GetString(pos++);
    }

    // ------------------------------------------------------------------------
    public static DateOnly GetDateOnly(SqlDataReader reader, ref int pos)
    {
        DateTime d = reader.GetDateTime(pos++);
        return new(d.Year, d.Month, d.Day);
    }

    public static DateTime GetDateTime(SqlDataReader reader, ref int pos)
    {
        DateTime d = reader.GetDateTime(pos++);
        return d;
    }

    // ------------------------------------------------------------------------
    // public static T getEnum<T>(SqlDataReader reader, ref int pos)
    //     where T : Enum
    // {
    //     return (T)Enum.ToObject(typeof(T), reader.GetInt32(pos++));
    // }

    // ========================================================================
    public static T GetDataObj<T>(SqlDataReader reader, ref int pos)
        where T : DataObj, new()
    {
        T info = new T();
        info.fetch(reader, ref pos);
        return info;
    }

    public static T GetDataObj<T>(SqlDataReader reader, int pos = 0)
        where T : DataObj, new()
    {
        return GetDataObj<T>(reader, ref pos);
    }

    // ========================================================================
}

/* EOF */
