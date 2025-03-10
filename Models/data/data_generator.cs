using Microsoft.Data.SqlClient;

// HACK: Đây là class dùng để chuyển đổi dữ liệu từ file csv sang cơ sở
// dữ liệu SQL Server.
sealed class DataGenerator
{
    // ========================================================================
    private static void drop_database(SqlConnection conn, string database_name)
    {
        string query =
            "SELECT name FROM sys.databases WHERE name NOT IN ('master', 'tempdb', 'model', 'msdb')";
        SqlCommand command = new SqlCommand(query, conn);

        List<string> databases = new List<string>();

        using (SqlDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                string result = reader["name"].ToString() ?? "";
                if (result != database_name)
                    continue;
                databases.Add(result);
                break;
            }
        }

        foreach (string database in databases)
        {
            string drop_query = $"DROP DATABASE [{database}]";
            Database.exec_non_query(conn, drop_query);
        }
    }

    // ========================================================================
    private static void create_database(SqlConnection conn, string database_name)
    {
        string query = $"CREATE DATABASE [{database_name}]";
        Database.exec_non_query(conn, query);
    }

    // ========================================================================
    private static void generate_1(SqlConnection conn)
    {
        string database_name = TableMngr.get_database_name();
        drop_database(conn, database_name);
        create_database(conn, database_name);
    }

    // ========================================================================
    private static void generate_2(SqlConnection conn)
    {
        drop_all_tables(conn);
        create_tables(conn);
    }

    // ========================================================================
    public static void generate()
    {
        Database.exec(generate_1, DatabaseUtils.get_server_only_conn_string());
        Database.exec(generate_2);
    }

    // ------------------------------------------------------------------------
    private static void drop_all_tables(SqlConnection conn)
    {
        Database.exec_non_query(
            conn,
            "EXEC sp_MSforeachtable @command1='ALTER TABLE ? NOCHECK CONSTRAINT ALL'"
        );

        List<string> table_names = new List<string>();

        using (
            SqlCommand command = new SqlCommand(
                "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'",
                conn
            )
        )
        {
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string tableName = reader["TABLE_NAME"].ToString() ?? "";
                    table_names.Add(tableName);
                }
            }
        }

        foreach (string tableName in table_names)
        {
            string query = $"DROP TABLE {tableName};";
            Database.exec_non_query(conn, query);
        }

        Database.exec_non_query(
            conn,
            "EXEC sp_MSforeachtable @command1='ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL'"
        );
    }

    // ========================================================================
    private static void create_tables(SqlConnection conn)
    {
        var table_configs = TableMngr.get_table_configs();

        foreach (var (_, table_config) in table_configs)
        {
            string query = $"CREATE TABLE {table_config.name} (";
            foreach (var field in table_config.fields)
            {
                query += $"{field.name} {field.sql_type},";
            }

            query = query.TrimEnd(',');
            query += ");";
            Database.exec_non_query(conn, query);
            string csv_file = table_config.csv_file;
            if (csv_file == "")
            {
                continue;
            }
            foreach (string line in File.ReadAllLines(csv_file))
            {
                RawQuery.insert(conn, line, table_config);
            }
        }
    }

    // ========================================================================
}

/* EOF */
