using Microsoft.Data.SqlClient;

class Test
{
    // ------------------------------------------------------------------------
    public static void test() => test6();

    // public static void test() => Test2.test();
    // ------------------------------------------------------------------------
    // INFO: Kiểm tra counter của bảng student
    private static void test5()
    {
        int count = 0;
        // Database.exec(conn => count = IdCounterQuery.increment(conn, Table.student));
        Console.WriteLine(count);
    }

    // ------------------------------------------------------------------------
    // INFO: Thêm mới student
    private static void test6()
    {
        Student new_student = new()
        {
            Username = "demo",
            Password = "demo",
            Name = "Nguyễn Văn A",
            Gender = Gender.male,
            Bday = new DateOnly(2012, 12, 12),
        };

        // Database.exec(conn => CommonQuery.insert_record_with_id(conn, new_student, Table.student));

        // IoUtils.print_list(
        //     Database.exec_list(conn => CommonQuery.get_all_records(conn, Table.student))
        // );
    }

    // ------------------------------------------------------------------------
}

/* EOF */
