using Microsoft.Data.SqlClient;

class Test
{
    // ------------------------------------------------------------------------
    public static void test() => test6();

    // public static void test() => Test2.test();

    // ------------------------------------------------------------------------
    // INFO: Tình huống in tên gia sư - tên môn học ra màn hinh
    // Sử dụng tính năng join của class Query.
    // ------------------------------------------------------------------------
    // INFO: In thông tin của student mang id = 1004
    private static void test2()
    {
        void func(SqlDataReader reader)
        {
            int pos = 0;
            var student = DataReader.get_data_obj<Student>(reader, ref pos);
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
            name = "Nguyễn Văn A",
            gender = InfoGender.MALE,
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
