using Microsoft.Data.SqlClient;

sealed class RawQuery
{
    // ------------------------------------------------------------------------
    public static string InsertQuery(
        List<List<string>> data,
        string table,
        DatabaseTableConfig table_config
    )
    {
        string query = $"INSERT INTO {table} VALUES ";
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
    public static void Insert(
        SqlConnection conn,
        List<List<string>> data,
        string table,
        DatabaseTableConfig table_config,
        int batch_size = 1000
    )
    {
        int total_batches = (int)Math.Ceiling((double)data.Count / batch_size);

        string queries = "";
        for (int batch_number = 0; batch_number < total_batches; batch_number++)
        {
            var batch = data.Skip(batch_number * batch_size).Take(batch_size).ToList();
            string query = InsertQuery(batch, table, table_config);
            if (query.Length + queries.Length > 1000000)
            {
                Database.exec_non_query(conn, queries);
                queries = query;
            }
            else
            {
                queries += $"{query} ";
            }
        }
        if (queries != "")
        {
            queries = queries.TrimEnd(' ');
            Database.exec_non_query(conn, queries);
        }
    }

    // ========================================================================
}

/* EOF */
