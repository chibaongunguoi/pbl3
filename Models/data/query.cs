using Microsoft.Data.SqlClient;

// INFO: Class tạo truy vấn
// Class truy vấn này được biệt hóa để sử dụng với kiểu
// dữ liệu Table (bảng) và Field (trường) được định nghĩa
// chỉ dành riêng cho dự án này.
sealed class Query
{
    // ========================================================================
    private Table table = new();

    // ========================================================================
    // INFO: Bắt đầu với một bảng
    public Query(Table table)
    {
        this.table = table;
    }

    // ------------------------------------------------------------------------
    // INFO: Trả về truy vấn INSERT
    public string get_insert_query<T>(T obj)
        where T : DataObj, new()
    {
        var table_config = TableMngr.get_table_config(table);
        List<string> parts = obj.ToListString();
        string query = $"INSERT INTO {table_config.name} VALUES (";
        int pos = 0;
        foreach (var field in table_config.fields)
        {
            switch (field.dtype)
            {
                case "INT":
                    query += $"{parts[pos]},";
                    break;
                case "NSTRING":
                    query += $"N'{parts[pos]}',";
                    break;
                case "STRING":
                    query += $"'{parts[pos]}',";
                    break;
            }
            if (++pos == parts.Count)
                break;
        }

        query = query.TrimEnd(',');
        query += ");";
        return query;
    }

    // ------------------------------------------------------------------------
    public void insert<T>(SqlConnection conn, T obj)
        where T : DataObj, new() => Database.exec_non_query(conn, get_insert_query<T>(obj));

    // ========================================================================
}

/* EOF */
