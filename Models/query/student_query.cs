namespace module_query;

using module_config;
using module_data;
using module_info;

class StudentQuery
{
    private static string get_table_name()
    {
        return Config.get_config("table", "student");
    }

    public static List<Student> get_all_students()
    {
        string query = $"SELECT * FROM {get_table_name()}";
        return DataFetcher<Student>.fetch(query);
    }

    public static List<Student> get_student_by_id(int id)
    {
        string query = $"SELECT * FROM {get_table_name()} WHERE id = {id}";
        return DataFetcher<Student>.fetch(query);
    }
}
