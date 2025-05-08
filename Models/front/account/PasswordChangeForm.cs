using System.ComponentModel.DataAnnotations;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

public class PasswordChangeForm
{
    [Required(ErrorMessage = "Nhập mật khẩu mới")]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nhập lại mật khẩu mới")]
    [DataType(DataType.Password)]
    [Compare("NewPassword", ErrorMessage = "Mật khẩu không khớp")]
    public string ConfirmPassword { get; set; } = string.Empty;

    public void Execute(SqlConnection conn, string username, string role, ITempDataDictionary tempData, out Account? account)
    {
        account = null;
        string table = role switch
        {
            UserRole.Student => Tbl.student,
            UserRole.Teacher => Tbl.teacher,
            _ => Tbl.admin
        };

        Query q = new(table);
        q.Set(Fld.password, NewPassword);
        q.Where(Fld.username, username);
        q.Update(conn);

        q = new(table);
        q.Where(Fld.username, username);
        var accounts = q.Select<Account>(conn);
        account = accounts.Count > 0 ? accounts[0] : null;
        if (account is null)
        {
            tempData["ErrorMessage"] = "Đổi mật khẩu thất bại";
        }
        else
        {
            tempData["SuccessMessage"] = "Đổi mật khẩu thành công";
        }
    }
}