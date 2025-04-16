using Microsoft.Data.SqlClient;

sealed class RawQuery
{
    // ------------------------------------------------------------------------
    public static string get_insert_query(List<List<string>> data, DatabaseTableConfig table_config)
    {
        string query = $"INSERT INTO {table_config.name} VALUES ";
        foreach (var record in data)
        {
            int pos = 0;
            string s = "(";
            foreach (var field in table_config.fields)
            {
                switch (field.dtype)
                {
                    case "INT":
                        s += $"{record[pos]},";
                        break;
                    case "NSTRING":
                        // strip the double quotes
                        s += $"N'{record[pos]}',";
                        break;
                    case "STRING":
                        s += $"'{record[pos]}',";
                        break;
                }
                if (++pos == record.Count)
                    break;
            }
            query += s.TrimEnd(',') + "),";
        }
        query = query.TrimEnd(',');
        query += ";";
        return query;
    }

    // ------------------------------------------------------------------------
    public static void insert(
        SqlConnection conn,
        List<List<string>> data,
        DatabaseTableConfig table_config,
        int batch_size = 1000
    )
    {
        int total_batches = (int)Math.Ceiling((double)data.Count / batch_size);

        for (int batch_number = 0; batch_number < total_batches; batch_number++)
        {
            var batch = data.Skip(batch_number * batch_size).Take(batch_size).ToList();
            string query = get_insert_query(batch, table_config);
            Database.exec_non_query(conn, query);
        }
    }

    // ========================================================================
}

/* EOF */
