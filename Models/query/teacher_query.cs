namespace module_query;

using module_config;
using module_database;
using module_info;

class TeacherQuery
{
    private static string get_table_name()
    {
        return Config.get_config("table", "teacher");
    }

    public static List<Teacher> get_all_teachers()
    {
        string query = $"SELECT * FROM {get_table_name()}";
        return DatabaseReceiver<Teacher>.get_query_results(query);
    }

    public static List<Teacher> get_teacher_by_id(int id)
    {
        string query = $"SELECT * FROM {get_table_name()} WHERE id = {id}";
        return DatabaseReceiver<Teacher>.get_query_results(query);
    }
}
