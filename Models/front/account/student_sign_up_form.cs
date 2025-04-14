using Microsoft.Data.SqlClient;

public class StudentSignUpForm
{
    public required string name { get; set; }
    public required string gender { get; set; }
    public DateOnly? bday { get; set; }
    public required string username { get; set; }
    public required string password { get; set; }
    public required string password_confirm { get; set; }

    //         bday_empty = "bday_empty",
    //         name_empty = "name_empty",
    //         username_empty = "username_empty",
    //         password_empty = "password_empty",
    //         password_mismatch = "password_mismatch",
    //         username_taken = "username_taken",
    //         run_out_of_id = "run_out_of_id";

    public class Log
    {
        public int? stu_id;
        public Dictionary<string, string> errors = new();
        public bool success => errors.Count == 0;
    }

    public Log execute(SqlConnection conn)
    {
        Log log = new Log();

        if (bday == null)
        {
            log.errors[ErrorKey.bday_empty] = "Ngày sinh không được để trống";
        }

        if (string.IsNullOrEmpty(name))
        {
            log.errors[ErrorKey.name_empty] = "Họ và tên không được để trống";
        }

        if (string.IsNullOrEmpty(username))
        {
            log.errors[ErrorKey.username_empty] = "Tên đăng nhập không được để trống";
        }

        if (string.IsNullOrEmpty(password))
        {
            log.errors[ErrorKey.password_empty] = "Mật khẩu không được để trống";
        }
        else if (password != password_confirm)
        {
            log.errors[ErrorKey.password_mismatch] = "Mật khẩu không khớp";
        }

        if (!log.success)
        {
            return log;
        }

        Query q = new(Table.student);
        q.where_(Field.student__username, username);
        int count = q.count(conn);

        if (count > 0)
        {
            log.errors[ErrorKey.username_taken] = "Tên đăng nhập đã tồn tại";
            return log;
        }

        int id = 0;
        if (!IdCounterQuery.increment(conn, Table.student, out id))
        {
            log.errors[ErrorKey.run_out_of_id] = "Đã hết ID cho bảng sinh viên";
            return log;
        }

        Student student = new()
        {
            id = id,
            username = username,
            password = password,
            name = name,
            gender = gender,
            bday = bday ?? new(),
        };
        q = new Query(Table.student);
        q.insert<Student>(conn, student);

        log.stu_id = id;
        return log;
    }

    public Log execute()
    {
        Log log = new();
        Database.exec(conn => log = execute(conn));
        return log;
    }
}
