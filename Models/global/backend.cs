namespace module_backend;

using module_config;
using module_info;
using module_query;

class Backend
{
    public static void start()
    {
        Config.load();
        test();
    }

    private static void test()
    {
        List<Student> students = StudentQuery.get_all_students();
        Console.WriteLine("Students:");
        foreach (Student student in students)
        {
            student.print();
        }
        List<Teacher> teachers = TeacherQuery.get_all_teachers();
        Console.WriteLine("Teachers:");
        foreach (Teacher teacher in teachers)
        {
            teacher.print();
        }
    }
}
