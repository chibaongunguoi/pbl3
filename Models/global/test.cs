using Microsoft.Data.SqlClient;

class Test
{
    // ------------------------------------------------------------------------
    public static void test() => test4();

    // ------------------------------------------------------------------------
    private static void test1()
    {
        void func(SqlDataReader reader)
        {
            string tch_name = DataReader.get_string(reader, 0);
            string sbj_name = DataReader.get_string(reader, 1);
            Console.WriteLine(tch_name + " - " + sbj_name);
        }
        Query q = new(Table.teacher_subject);
        q.join(Field.teacher__id, Field.teacher_subject__tch_id);
        q.join(Field.subject__id, Field.teacher_subject__sbj_id);
        q.output(Field.teacher__fullname);
        q.output(Field.subject__name);
        q.select(func);
    }

    // ------------------------------------------------------------------------
    private static void test2()
    {
        void func(SqlDataReader reader)
        {
            var student = DataReader.get_data_obj<Student>(reader);
            Console.WriteLine(student.ToString());
        }
        Query q = new(Table.student);
        q.where_(Field.student__id, 1004);
        q.select(func);
    }

    // ------------------------------------------------------------------------
    private static void test3()
    {
        Query q = new(Table.student);
        q.where_(Field.student__id, 1004);
        foreach (var r in q.select<Student>())
        {
            Console.WriteLine(r.ToString());
        }
    }

    // ------------------------------------------------------------------------
    private static void test4()
    {
        List<InfoInterval> intervals_1 = new()
        {
            new()
            {
                day = InfoDay.TUESDAY,
                start = { hour = 8, minute = 0 },
                end = { hour = 10, minute = 30 },
            },
        };
        List<InfoInterval> intervals_2 = new()
        {
            new()
            {
                day = InfoDay.TUESDAY,
                start = { hour = 8, minute = 15 },
                end = { hour = 9, minute = 15 },
            },
            new()
            {
                day = InfoDay.TUESDAY,
                start = { hour = 9, minute = 30 },
                end = { hour = 10, minute = 15 },
            },
        };
        IoUtils.print_list(ScheduleUtils.get_subtraction(intervals_1, intervals_2));
    }

    // ------------------------------------------------------------------------
    private static void test5()
    {
        IoUtils.print_list(TeacherScheduleQuery.get_avai_schedule(2013));
    }

    // ------------------------------------------------------------------------
}

/* EOF */
