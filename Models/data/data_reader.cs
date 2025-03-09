using Microsoft.Data.SqlClient;

// INFO: Tập hợp các hàm dùng để tạo đối tượng từ một SqlDataReader.
// Cần xác định pos là chỉ số cột bắt đầu để đọc dữ liệu.
sealed class DataReader
{
    // ========================================================================
    public static int get_int(SqlDataReader reader, int pos = 0)
    {
        return reader.GetInt32(pos);
    }

    // ------------------------------------------------------------------------
    public static string get_string(SqlDataReader reader, int pos = 0)
    {
        return reader.GetString(pos);
    }

    // ------------------------------------------------------------------------
    public static T get_enum<T>(SqlDataReader reader, int pos = 0)
        where T : Enum
    {
        return (T)Enum.ToObject(typeof(T), reader.GetInt32(pos));
    }

    // ========================================================================
    public static T get_data_obj<T>(SqlDataReader reader, int pos = 0)
        where T : DataObj, new()
    {
        T info = new T();
        info.fetch_data(reader);
        return info;
    }

    // ========================================================================
    // INFO:  Đọc thông tin trên reader và thêm thông tin vào tham chiếu list các DataObj.
    public static void read<T>(SqlDataReader reader, ref List<T> result)
        where T : DataObj, new()
    {
        result.Add(get_data_obj<T>(reader));
    }

    // ------------------------------------------------------------------------
    // INFO: Đọc thông tin trên reader và thêm thông tin vào tham chiếu list các string.
    public static void read(SqlDataReader reader, ref List<string> result)
    {
        List<string> record = new();
        for (int i = 0; i < reader.FieldCount; i++)
        {
            record.Add(reader[i].ToString() ?? "");
        }

        result.Add(string.Join(",", record));
    }

    // ========================================================================
}

/* EOF */
