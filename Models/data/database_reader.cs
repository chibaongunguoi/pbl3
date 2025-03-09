using Microsoft.Data.SqlClient;

class DatabaseReader
{
    // ========================================================================
    // INFO:  Đọc thông tin trên reader và thêm thông tin vào tham chiếu list các DataObj.
    public static void read<T>(SqlDataReader reader, ref List<T> result)
        where T : DataObj, new()
    {
        T info = new T();
        info.fetch_data(reader);
        result.Add(info);
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
