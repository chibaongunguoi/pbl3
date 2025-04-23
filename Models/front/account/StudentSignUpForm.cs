using Microsoft.Data.SqlClient;

public class StudentSignUpForm
{
    public required string Name { get; set; }
    public required string Gender { get; set; }
    public DateOnly? Bday { get; set; }
    public required string Tel { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string PasswordConfirm { get; set; }

    public class Log
    {
        public int? stu_id;
        public Dictionary<string, string> errors = new();
        public bool success => errors.Count == 0;
    }

    public Log execute(SqlConnection conn)
    {
        Log log = new Log();

        if (Bday == null)
        {
            log.errors[ErrorKey.bday_empty] = "Ngày sinh không được để trống";
        }

        if (string.IsNullOrEmpty(Name))
        {
            log.errors[ErrorKey.name_empty] = "Họ và tên không được để trống";
        }

        if (string.IsNullOrEmpty(Username))
        {
            log.errors[ErrorKey.username_empty] = "Tên đăng nhập không được để trống";
        }

        if (string.IsNullOrEmpty(Password))
        {
            log.errors[ErrorKey.password_empty] = "Mật khẩu không được để trống";
        }
        else if (Password != PasswordConfirm)
        {
            log.errors[ErrorKey.password_mismatch] = "Mật khẩu không khớp";
        }

        if (!log.success)
        {
            return log;
        }

        Query q = new(Tbl.student);
        q.Where(Fld.username, Username);
        int count = q.Count(conn);

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
            Id = id,
            Username = Username,
            Password = Password,
            Name = Name,
            Gender = Gender,
            Bday = Bday ?? new(),
            Tel = Tel,
        };
        Query q1 = new(Tbl.student);
        q1.Insert(conn, string.Join(", ", student.ToList()));

        log.stu_id = id;
        return log;
    }

    public Log execute()
    {
        Log log = new();
        QDatabase.Exec(conn => log = execute(conn));
        return log;
    }
}
