using System.Diagnostics;
using System.Text.Json;
using Microsoft.Data.SqlClient;

sealed class DataGenerator
{
    // ========================================================================
    public static void generate(string server_name, string database_name)
    {
        QDatabase.Exec(conn => drop_database(conn, database_name), server_only: true);
        QDatabase.Exec(conn => create_tables(conn));
    }

    // ========================================================================
    private static void drop_database(SqlConnection conn, string database_name)
    {
        string query =
            $"SELECT name FROM sys.databases WHERE name NOT IN ('master', 'tempdb', 'model', 'msdb') AND name = '{database_name}'";

        string? result = null;
        QDatabase.ExecQuery(conn, query, reader => result = DataReader.getStr(reader));
        if (result != null)
            QDatabase.ExecQuery(
                conn,
                $"DROP DATABASE [{database_name}] CREATE DATABASE [{database_name}]"
            );
        else
            QDatabase.ExecQuery(conn, $"CREATE DATABASE [{database_name}]");
    }

    // ========================================================================
    private static void create_tables(SqlConnection conn)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        var table_configs = TableMngr.get_table_configs();

        List<string> queries = new List<string>();
        foreach (var (table, table_config) in table_configs)
        {
            string query = $"CREATE TABLE {table} (";
            foreach (var field in table_config.fields)
            {
                query += $"{field.name} {field.sql_type},";
            }

            foreach (var fk in table_config.foreign_keys)
            {
                query += $"FOREIGN KEY ({fk.field}) REFERENCES {fk.ref_table}({fk.ref_field}),";
            }

            query = query.TrimEnd(',');
            query += ");";
            queries.Add(query);

            string json_file = table_config.json_file;
            if (json_file == "")
            {
                continue;
            }
            string database_config_json = File.ReadAllText(json_file);
            var lst = JsonSerializer.Deserialize<List<string>>(database_config_json) ?? new();
            RawQuery.GetInsertQueries(ref lst, table, ref queries);
        }

        string big_query = "";

        foreach (var query in queries)
        {
            big_query += $" {query}";
            if (big_query.Length > 1000000)
            {
                QDatabase.ExecQuery(conn, big_query);
                big_query = "";
            }
        }

        if (big_query.Length > 0)
        {
            QDatabase.ExecQuery(conn, big_query);
        }

        stopwatch.Stop();
        TimeSpan elapsed = stopwatch.Elapsed;
        Console.WriteLine($"DataGenerator - Time taken: {elapsed.TotalMilliseconds} ms");
    }

    // ========================================================================
}

/* EOF */
