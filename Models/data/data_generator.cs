namespace module_data;

using Microsoft.Data.SqlClient;
using module_config;

class DataGenerator : DatabaseConnection
{
    // ========================================================================
    public static void run()
    {
        DataGenerator generator = new DataGenerator();
        generator.send();
    }

    // ------------------------------------------------------------------------
    private static void drop_all_tables(SqlConnection connection)
    {
        DatabaseConnection.run_query(
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
            DatabaseConnection.run_query(connection, query);
        }

        DatabaseConnection.run_query(
            connection,
            "EXEC sp_MSforeachtable @command1='ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL'"
        );
        Console.WriteLine("All tables dropped successfully");
    }

    // ========================================================================
    private static void create_demo_user_table(SqlConnection connection)
    {
        string table_name = Config.get_config("table", "demo_user");
        string query =
            $"CREATE TABLE {table_name} ("
            + "id INT PRIMARY KEY NOT NULL,"
            + "name NVARCHAR(50) NOT NULL,"
            + ");";
        DatabaseConnection.run_query(connection, query);
        foreach (string line in File.ReadAllLines("data/demo_user.csv"))
        {
            string[] parts = line.Split(',');
            string id = parts[0];
            string name = parts[1];
            DatabaseConnection.run_query(
                connection,
                $"INSERT INTO {table_name} VALUES ({id}, N'{name}');"
            );
        }
    }

    // ========================================================================
    private void send()
    {
        connect();
    }

    // ========================================================================
    protected override void action(SqlConnection connection)
    {
        DataGenerator.drop_all_tables(connection);
        DataGenerator.create_demo_user_table(connection);
    }

    // ========================================================================
}

/* EOF */
