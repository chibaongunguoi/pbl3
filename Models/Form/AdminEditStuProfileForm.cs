using System.ComponentModel.DataAnnotations;
using Microsoft.Data.SqlClient;

public class AdminEditStuProfileForm
{
    public int StuId { get; set; }
    public string? OldUsername { get; set; }
    [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "Họ và tên không được để trống")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Giới tính không được để trống")]
    public string? Gender { get; set; }

    [Required(ErrorMessage = "Ngày sinh không được để trống")]
    public DateOnly? BDay { get; set; }

    [Required(ErrorMessage = "Số điện thoại không được để trống")]
    public string? Tel { get; set; }
    public string? NewPassword { get; set; }
    public string? ConfirmPassword { get; set; }

    public Dictionary<string, string> Message { get; set; } = new();

    public AdminEditStuProfileForm()
    { }
    public AdminEditStuProfileForm(int stuId)
    {
        StuId = stuId;
        Query q = new(Tbl.student);
        q.Output(Field.student__username);
        q.Output(Field.student__name);
        q.Output(Field.student__gender);
        q.Output(Field.student__bday);
        q.Output(Field.student__tel);
        q.Where(Field.student__id, stuId);

        QDatabase.Exec(conn => q.Select(conn, reader =>
        {
            int pos = 0;
            Username = QDataReader.GetString(reader, ref pos);
            Name = QDataReader.GetString(reader, ref pos);
            Gender = QDataReader.GetString(reader, ref pos);
            BDay = QDataReader.GetDateOnly(reader, ref pos);
            Tel = QDataReader.GetString(reader, ref pos);
        }));
    }

    public void Execute(SqlConnection conn, int stuId)
    {
        if (Username is null || Name is null || Gender is null || BDay is null || Tel is null)
            return;

        string oldUsername = string.Empty;
        Query q = new(Tbl.student);
        q.Output(Field.student__username);
        q.Where(Field.student__id, stuId);
        q.Select(conn, reader =>
        {
            oldUsername = QDataReader.GetString(reader);
        });

        if (oldUsername != Username)
            foreach (string table in new List<string> { Tbl.student, Tbl.teacher, Tbl.admin })
            {
                q = new(table);
                q.Where(Fld.username, Username);
                if (q.Count(conn) > 0)
                {
                    Message.Add("Error", "Tên đăng nhập đã tồn tại");
                    return;
                }
            }


        q = new(Tbl.student);
        q.Set(Field.student__username, Username);
        q.SetNString(Field.student__name, Name);
        q.Set(Field.student__gender, Gender);
        q.Set(Field.student__bday, BDay);
        q.Set(Field.student__tel, Tel);

        if (NewPassword is not null || ConfirmPassword is not null)
        {
            if (NewPassword != ConfirmPassword)
            {
                Message.Add("Error", "Mật khẩu không khớp");
                return;
            }
            if (NewPassword is not null)
                q.Set(Field.student__password, NewPassword);
        }

        q.Where(Field.student__id, stuId);
        q.Update(conn);
        Message.Add("Success", "Cập nhật thành công");
    }
}