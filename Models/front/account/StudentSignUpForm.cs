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
}
