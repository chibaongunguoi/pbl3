using Microsoft.Data.SqlClient;

class Test
{
    // ------------------------------------------------------------------------
    public static void test() => test6();

    // public static void test() => Test2.test2();

    // ------------------------------------------------------------------------
    // INFO: Tình huống in tên gia sư - tên môn học ra màn hinh
    // Sử dụng tính năng join của class Query.
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
        Database.exec(conn => q.select(conn, func));
    }

    // ------------------------------------------------------------------------
    // INFO: In thông tin của student mang id = 1004
    private static void test2()
    {
        void func(SqlDataReader reader)
        {
            var student = DataReader.get_data_obj<Student>(reader);
            Console.WriteLine(student.ToString());
        }
        Query q = new(Table.student);
        q.where_(Field.student__id, 1004);
        Database.exec(conn => q.select(conn, func));
    }

    // ------------------------------------------------------------------------
    // INFO: In thông tin của student mang id = 1004
    private static void test3()
    {
        Query q = new(Table.student);
        q.where_(Field.student__id, 1004);
        List<Student> students = new();
        Database.exec_list(conn => students = q.select<Student>(conn));
        foreach (var r in students)
        {
            Console.WriteLine(r.ToString());
        }
    }

    // ------------------------------------------------------------------------
    // INFO: Thêm và bớt TeacherSchedule
    private static void test4()
    {
        int tch_id = 2001;
        int sch_id = 42;
        TeacherSchedule demo_schedule = new() { tch_id = tch_id, sch_id = sch_id };
        void func(SqlConnection conn)
        {
            var current_schedule = TeacherScheduleQuery.get_avai_schedule(conn, tch_id);
            IoUtils.print_list(current_schedule);

            TeacherScheduleQuery.add_schedule(conn, demo_schedule);

            var current_schedule_2 = TeacherScheduleQuery.get_avai_schedule(conn, tch_id);
            IoUtils.print_list(current_schedule_2);

            TeacherScheduleQuery.remove_schedule(conn, demo_schedule);

            var current_schedule_3 = TeacherScheduleQuery.get_avai_schedule(conn, tch_id);
            IoUtils.print_list(current_schedule_3);
        }

        Database.exec(func);
    }

    // ------------------------------------------------------------------------
    // INFO: Kiểm tra counter của bảng student
    private static void test5()
    {
        int count = 0;
        Database.exec(conn => count = IdCounterQuery.increment(conn, Table.student));
        Console.WriteLine(count);
    }

    // ------------------------------------------------------------------------
    // INFO: Thêm mới student
    private static void test6()
    {
        Student new_student = new()
        {
            username = "demo",
            password = "demo",
            fullname = "Nguyễn Văn A",
            gender = InfoGender.MALE,
            addr = "123 Đường Chân Trời",
            tel = "0987654321",
            bday = new InfoDate
            {
                year = 2012,
                month = 12,
                day = 12,
            },
        };

        Database.exec(conn => CommonQuery.insert_record_with_id(conn, new_student, Table.student));

        IoUtils.print_list(
            Database.exec_list(conn => CommonQuery.get_all_records(conn, Table.student))
        );
    }

    // ------------------------------------------------------------------------
}

/* EOF */
