using System.ComponentModel.DataAnnotations;

public class LoginForm
{
    [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
    public required string Username { get; set; } = string.Empty;
    [Required(ErrorMessage = "Mật khẩu không được để trống")]
    [DataType(DataType.Password)]
    public required string Password { get; set; } = string.Empty;
}

/* EOF */
