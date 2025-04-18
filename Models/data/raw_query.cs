sealed class RawQuery
{
    // ------------------------------------------------------------------------
    public static string insertQuery(
        ref List<List<string>> data,
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
    public static void getInsertQueries( // thực hiện nhiều bản ghi mà gọi insertQuery
        ref List<List<string>> data,
        string table,
        DatabaseTableConfig table_config,
        ref List<string> queries,
        int batch_size = 1000
    )
    {
        int total_batches = (int)Math.Ceiling((double)data.Count / batch_size);

        for (int batch_number = 0; batch_number < total_batches; batch_number++)
        {
            var batch = data.Skip(batch_number * batch_size).Take(batch_size).ToList();
            string query = insertQuery(ref batch, table, table_config);
            queries.Add(query);
        }
    }

    // ========================================================================
}

/* EOF */
