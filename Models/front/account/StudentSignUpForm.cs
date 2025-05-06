using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;


public class StudentSignUpForm
{
    [Required(ErrorMessage = "Họ và tên không được để trống")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "Giới tính không được để trống")]
    public required string Gender { get; set; }

    [Required(ErrorMessage = "Ngày sinh không được để trống")]
    [DataType(DataType.Date)]
    public DateOnly? Bday { get; set; }

    [Required(ErrorMessage = "Số điện thoại không được để trống")]
    [RegularExpression(@"^\d{10,11}$", ErrorMessage = "Số điện thoại không hợp lệ")]
    [DataType(DataType.PhoneNumber)]
    public required string Tel { get; set; }

    [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
    // [RegularExpression(@"^[a-zA-Z0-9_]{5,20}$", ErrorMessage = "Tên đăng nhập không hợp lệ")]
    public required string Username { get; set; }
    
    [Required(ErrorMessage = "Mật khẩu không được để trống")]
    [DataType(DataType.Password)]
    // [RegularExpression(@"^[a-zA-Z0-9_]{5,20}$", ErrorMessage = "Mật khẩu không hợp lệ")]
    public required string Password { get; set; }
    
    [Required(ErrorMessage = "Xác nhận mật khẩu không được để trống")]
    [Compare(nameof(Password), ErrorMessage = "Mật khẩu không khớp")]
    [DataType(DataType.Password)]
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
