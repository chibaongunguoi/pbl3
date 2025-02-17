namespace module_data;

using Microsoft.Data.SqlClient;
using module_config;

sealed class DataGenerator
{
    // ========================================================================
    private static void process_connection(SqlConnection connection)
    {
        DataGenerator.drop_all_tables(connection);
        DataGenerator.create_demo_user_table(connection);
    }

    // ========================================================================
    public static void generate()
    {
        Database.call_func_new_conn(DataGenerator.process_connection);
    }

    // ------------------------------------------------------------------------
    private static void drop_all_tables(SqlConnection connection)
    {
        Database.run_query_with_conn(
            connection,
            "EXEC sp_MSforeachtable @command1='ALTER TABLE ? NOCHECK CONSTRAINT ALL'"
        );

        List<string> tableNames = new List<string>();

        using (
            SqlCommand command = new SqlCommand(
                "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'",
                connection
            )
        )
        {
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string tableName = reader["TABLE_NAME"].ToString() ?? "";
                    tableNames.Add(tableName);
                }
            }
        }

        foreach (string tableName in tableNames)
        {
            string query = $"DROP TABLE {tableName};";
            Database.run_query_with_conn(connection, query);
        }

        Database.run_query_with_conn(
            connection,
            "EXEC sp_MSforeachtable @command1='ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL'"
        );
    }

    // ========================================================================
    private static void create_demo_user_table(SqlConnection connection)
    {
        string table_name = Config.get_config("tableNames", "demo_user");
        string query =
            $"CREATE TABLE {table_name} ("
            + "id INT PRIMARY KEY NOT NULL,"
            + "username NVARCHAR(50) NOT NULL,"
            + "password NVARCHAR(50) NOT NULL,"
            + "name NVARCHAR(50) NOT NULL,"
            + "working_time TIME NOT NULL,"
            + ");";
        Database.run_query_with_conn(connection, query);
        foreach (string line in File.ReadAllLines("data/demo_user.csv"))
        {
            string[] parts = line.Split(',');
            int pos = 0;
            string id = parts[pos++];
            string username = parts[pos++];
            string password = parts[pos++];
            string name = parts[pos++];
            string working_time = parts[pos++];
            Database.run_query_with_conn(
                connection,
                $"INSERT INTO {table_name} VALUES ({id}, '{username}', '{password}', N'{name}', ('{working_time}'));"
            );
        }
    }
    // ========================================================================
}

/* EOF */
