using Microsoft.Data.SqlClient;

public class StudentSignUpForm
{
    public required string name { get; set; }
    public required string gender { get; set; }
    public DateOnly? bday { get; set; }
    public required string username { get; set; }
    public required string password { get; set; }
    public required string password_confirm { get; set; }

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

        Query q = new(Tbl.student);
        q.Where(Fld.username, username);
        int count = q.count(conn);

        if (count > 0)
        {
            log.errors[ErrorKey.username_taken] = "Tên đăng nhập đã tồn tại";
            return log;
        }

        int id = 0;
        if (!IdCounterQuery.increment(conn, Tbl.student, out id))
        {
            log.errors[ErrorKey.run_out_of_id] = "Đã đạt giới hạn số lượng học viên";
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
        Query q1 = new(Tbl.student);
        q1.insert(conn, string.Join(", ", student.ToList()));

        log.stu_id = id;
        return log;
    }

    public Log execute()
    {
        Log log = new();
        QDatabase.exec(conn => log = execute(conn));
        return log;
    }
}
