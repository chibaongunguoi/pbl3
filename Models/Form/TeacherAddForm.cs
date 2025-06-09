using System.ComponentModel.DataAnnotations;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

public class TeacherAddForm
{
    public Dictionary<string, string> Message { get; } = new();

    [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
    public string? Username { get; set; } = null;
    
    [Required(ErrorMessage = "Mật khẩu không được để trống")]
    [DataType(DataType.Password)]
    public string? Password { get; set; } = null;
    
    [Required(ErrorMessage = "Xác nhận mật khẩu không được để trống")]
    [Compare(nameof(Password), ErrorMessage = "Mật khẩu không khớp")]
    [DataType(DataType.Password)]
    public string? PasswordConfirm { get; set; } = null;

    [Required(ErrorMessage = "Họ và tên không được để trống")]
    public string? Name { get; set; } = null;    [Required(ErrorMessage = "Giới tính không được để trống")]
    public string? MGender { get; set; } = Gender.male;

    [Required(ErrorMessage = "Ngày sinh không được để trống")]
    [DataType(DataType.Date)]
    public DateOnly? Bday { get; set; } = null;

    [Required(ErrorMessage = "Số điện thoại không được để trống")]
    public string? Tel { get; set; } = null;

    public string? Description { get; set; } = "";    
    public void Execute(SqlConnection conn, ModelStateDictionary modelState, out Teacher? teacher)
    {
        teacher = null;

        if (Username is null || Name is null || MGender is null || Bday is null || Tel is null)
        {
            modelState.AddModelError(string.Empty, "Thông tin không được để trống");
            Message.Add("Error", "Thông tin không được để trống");
            return;
        }

        // Check if username already exists
        foreach (string table in new List<string> { Tbl.student, Tbl.teacher, Tbl.admin })
        {
            Query q = new(table);
            q.Where(Fld.username, Username);
            if (q.Count(conn) > 0)
            {
                modelState.AddModelError(nameof(Username), "Tên đăng nhập đã tồn tại");
                Message.Add("Error", "Tên đăng nhập đã tồn tại");
                return;
            }
        }

        // Create new teacher record
        if (!IdCounterQuery.increment(conn, Tbl.teacher, out int id))
        {
            modelState.AddModelError(string.Empty, "Đã đạt giới hạn số lượng giảng viên");
            Message.Add("Error", "Đã đạt giới hạn số lượng giảng viên");
            return;
        }
        string password = BackUtils.ComputeSHA256(Password ?? "");
        // Create a teacher object to insert
        Teacher newTeacher = new()
        {
            Id = id,
            Username = Username ?? "",
            Password = password,
            Name = Name ?? "",
            Gender = MGender ?? "",
            Bday = Bday ?? DateOnly.FromDateTime(DateTime.Now),
            Tel = Tel ?? "",
            Description = Description ?? ""
        };

        Query query = new(Tbl.teacher);
        query.Insert(conn, newTeacher);
        teacher = newTeacher;
        Message.Add("Success", "Thêm giảng viên thành công");
    }
}
